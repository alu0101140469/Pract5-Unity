
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Clase base para los objetos que se seleccionan manteniendo la mirada.
/// Compatible con CardboardReticlePointer y las interfaces de Unity.
/// </summary>
public abstract class GazeDwellTarget : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Min(0.1f)]
    private float dwellSeconds = 1.0f;

    private bool isGazed;
    private bool selectedDuringThisGaze;
    private float elapsed;

    // Métodos llamados por CardboardReticlePointer mediante SendMessage.
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
        isGazed = true;
        selectedDuringThisGaze = false;
        elapsed = 0f;
        OnGazeStarted();
    }

    private void HandlePointerExit()
    {
        isGazed = false;
        selectedDuringThisGaze = false;
        elapsed = 0f;
        OnGazeEnded();
    }

    protected virtual void Update()
    {
        if (!isGazed || selectedDuringThisGaze)
            return;

        elapsed += Time.deltaTime;

        if (elapsed >= dwellSeconds)
        {
            selectedDuringThisGaze = true;
            OnGazeSelected();
        }
    }

    protected virtual void OnGazeStarted() { }

    protected virtual void OnGazeEnded() { }

    /// <summary>
    /// Acción que realiza cada objeto al completar la mirada.
    /// </summary>
    protected abstract void OnGazeSelected();
}
