using System.Collections;
using UnityEngine;

/// <summary>
/// Recolectable que se guarda al mantener la mirada sobre él.
/// Cuando se recupera, vuelve a mostrarse y vuela hacia el jugador.
/// </summary>
public class GazePickup : GazeDwellTarget
{
    [Header("Referencias")]
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private Renderer[] visuals;
    [SerializeField] private Collider[] hitColliders;

    [Header("Animación de retorno")]
    [SerializeField, Min(0f)] private float rotationSpeed = 180f;

    private bool isCollected;
    private bool isReturning;

    private void Awake()
    {
        if (visuals == null || visuals.Length == 0)
            visuals = GetComponentsInChildren<Renderer>(true);

        if (hitColliders == null || hitColliders.Length == 0)
            hitColliders = GetComponentsInChildren<Collider>(true);
    }

    protected override void OnGazeSelected()
    {
        if (isCollected || isReturning)
            return;

        if (inventory == null)
            inventory = FindAnyObjectByType<InventoryManager>();

        if (inventory == null)
        {
            Debug.LogError(
                $"[{nameof(GazePickup)}] No se ha asignado InventoryManager en {name}.",
                this);
            return;
        }

        isCollected = true;
        SetVisuals(false);
        SetColliders(false);
        inventory.RegisterCollected(this);

        Debug.Log($"Recolectado: {name}", this);
    }

    /// <summary>
    /// Muestra el objeto y lo desplaza hacia el objetivo indicado.
    /// La corrutina termina cuando el objeto llega a la proximidad del jugador.
    /// </summary>
    public IEnumerator FlyToPlayer(Transform target, float speed)
    {
        if (target == null)
            yield break;

        isReturning = true;
        SetVisuals(true);

        const float arrivalDistance = 0.25f;

        while (Vector3.Distance(transform.position, target.position) > arrivalDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime);

            transform.Rotate(
                Vector3.up,
                rotationSpeed * Time.deltaTime,
                Space.World);

            yield return null;
        }

        SetVisuals(false);
        isReturning = false;
    }

    private void SetVisuals(bool visible)
    {
        if (visuals == null)
            return;

        foreach (Renderer visual in visuals)
        {
            if (visual != null)
                visual.enabled = visible;
        }
    }

    private void SetColliders(bool enabledState)
    {
        if (hitColliders == null)
            return;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider != null)
                hitCollider.enabled = enabledState;
        }
    }
}
