
using UnityEngine;
using UnityEngine.InputSystem;

public class EditorWASDController : MonoBehaviour
{
    [Header("Referencias")]
    public CharacterController controller;
    public Transform head;
    public GazeWalkController gazeController;

    [Header("Movimiento")]
    public float moveSpeed = 4f;
    public float gravity = -9.81f;

    [Header("Camara")]
    public float mouseSensitivity = 0.15f;
    public float maxLookAngle = 80f;

    private float verticalVelocity;
    private float pitch;

    private void Awake()
    {
#if UNITY_EDITOR
        // Evitamos que el movimiento por mirada
        // interfiera con el movimiento WASD.
        if (gazeController != null)
            gazeController.enabled = false;
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR
        HandleMouseLook();
        HandleMovement();
#endif
    }

    private void HandleMouseLook()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            Cursor.lockState = CursorLockMode.Locked;

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
            Cursor.lockState = CursorLockMode.None;

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float yaw = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        transform.Rotate(Vector3.up, yaw);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        head.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        if (controller == null || head == null)
            return;

        float x = 0f;
        float z = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
        }

        Vector3 forward = head.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = head.right;
        right.y = 0f;
        right.Normalize();

        Vector3 movement = (forward * z + right * x).normalized;

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMovement =
            movement * moveSpeed +
            Vector3.up * verticalVelocity;

        controller.Move(finalMovement * Time.deltaTime);
    }
}
