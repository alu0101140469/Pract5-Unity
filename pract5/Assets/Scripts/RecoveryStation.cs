using UnityEngine;

/// <summary>
/// Estación que recupera todos los objetos guardados cuando se mira durante
/// el tiempo de permanencia configurado en GazeDwellTarget.
/// </summary>
public class RecoveryStation : GazeDwellTarget
{
    [SerializeField] private InventoryManager inventory;

    protected override void OnGazeSelected()
    {
        if (inventory == null)
            inventory = FindAnyObjectByType<InventoryManager>();

        if (inventory == null)
        {
            Debug.LogError(
                $"[{nameof(RecoveryStation)}] No se ha encontrado InventoryManager.",
                this);
            return;
        }

        inventory.RecallCollected();
    }
}
