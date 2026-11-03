using UnityEngine;

/// <summary>
/// Movimiento del jugador mientras el puntero de Cardboard está sobre
/// una superficie que contiene GazeWalkSurface.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class GazeWalkController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform head;

    [Header("Movimiento")]
    [SerializeField, Min(0f)] private float moveSpeed = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    private bool gazingAtWalkableSurface;
    private float verticalSpeed;

    private void Reset()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (head == null && Camera.main != null)
            head = Camera.main.transform;
    }

    public void SetWalkingFromGaze(bool isLookingAtSurface)
    {
        gazingAtWalkableSurface = isLookingAtSurface;
    }

    private void Update()
    {
        if (characterController == null || head == null)
            return;

        Vector3 horizontalMovement = Vector3.zero;

        if (gazingAtWalkableSurface)
        {
            // Se utiliza el rumbo horizontal de la cámara, sin inclinar al jugador
            // hacia arriba o hacia abajo al mirar alrededor.
            Vector3 forward = head.forward;
            forward.y = 0f;

            if (forward.sqrMagnitude > 0.001f)
            {
                forward.Normalize();
                horizontalMovement = forward * moveSpeed;
            }
        }

        if (characterController.isGrounded && verticalSpeed < 0f)
            verticalSpeed = -2f;

        verticalSpeed += gravity * Time.deltaTime;

        Vector3 finalMovement =
            horizontalMovement + Vector3.up * verticalSpeed;

        characterController.Move(finalMovement * Time.deltaTime);
    }
}
