
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Activa el movimiento por mirada mientras el retículo apunta al terreno.
/// Compatible con CardboardReticlePointer y las interfaces de Unity.
/// </summary>
public class GazeWalkSurface : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GazeWalkController walker;

    // Métodos públicos para CardboardReticlePointer (SendMessage).
    public void OnPointerEnter()
    {
        HandlePointerEnter();
    }

    public void OnPointerExit()
    {
        HandlePointerExit();
    }

    // Implementación explícita de las interfaces de Unity.
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        HandlePointerEnter();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        HandlePointerExit();
    }

    private void HandlePointerEnter()
    {
        if (walker != null)
            walker.SetWalkingFromGaze(true);
    }

    private void HandlePointerExit()
    {
        if (walker != null)
            walker.SetWalkingFromGaze(false);
    }

    private void OnDisable()
    {
        if (walker != null)
            walker.SetWalkingFromGaze(false);
    }
}
