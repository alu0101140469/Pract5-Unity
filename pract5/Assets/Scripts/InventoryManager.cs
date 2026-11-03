using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Guarda los objetos recogidos y coordina su retorno hacia el jugador.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [Header("Destino de los objetos")]
    [SerializeField] private Transform playerTarget;
    [SerializeField, Min(0.1f)] private float returnSpeed = 3.0f;

    private readonly List<GazePickup> collectedItems = new List<GazePickup>();
    private bool recallInProgress;
    private int returnedCount;

    public int CollectedCount => collectedItems.Count;
    public int ReturnedCount => returnedCount;

    public void RegisterCollected(GazePickup pickup)
    {
        if (pickup == null || collectedItems.Contains(pickup))
            return;

        collectedItems.Add(pickup);
        Debug.Log($"Inventario: {collectedItems.Count} objeto(s) pendiente(s).", this);
    }

    /// <summary>Inicia simultáneamente el retorno de todos los objetos recogidos.</summary>
    public void RecallCollected()
    {
        if (recallInProgress)
        {
            Debug.Log("La recuperación ya está en curso.", this);
            return;
        }

        if (playerTarget == null)
        {
            Debug.LogError(
                "Asigna la cámara del jugador en Player Target del InventoryManager.",
                this);
            return;
        }

        if (collectedItems.Count == 0)
        {
            Debug.Log("No hay objetos pendientes de recuperar.", this);
            return;
        }

        recallInProgress = true;

        // Se copia la lista porque las corrutinas eliminarán elementos de la original.
        List<GazePickup> pending = new List<GazePickup>(collectedItems);

        foreach (GazePickup item in pending)
        {
            if (item != null)
                StartCoroutine(ReturnOne(item));
        }
    }

    private IEnumerator ReturnOne(GazePickup item)
    {
        yield return item.FlyToPlayer(playerTarget, returnSpeed);

        collectedItems.Remove(item);
        returnedCount++;

        Debug.Log(
            $"Objeto recuperado: {item.name}. Total recuperados: {returnedCount}.",
            this);

        if (collectedItems.Count == 0)
        {
            recallInProgress = false;
            Debug.Log("Todos los objetos han regresado al jugador.", this);
        }
    }
}
