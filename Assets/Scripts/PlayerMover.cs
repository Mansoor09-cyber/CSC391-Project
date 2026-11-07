using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float sprintSpeed = 9.5f;
    public float jumpHeight = 1.6f;
    public float gravity = -20f;
    public float rotationSpeed = 12f;
    public Transform cameraTransform;
    public GameManager manager;

    [Header("Status (Read-Only)")]
    public bool isGrounded { get; private set; }
    public bool isMoving { get; private set; }
    public bool isSprinting { get; private set; }
    public float currentSpeed { get; private set; }

    private CharacterController controller;
    private float verticalSpeed = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Stop movement when game is over
        if (manager != null && manager.gameOver)
        {
            isMoving = false;
            isSprinting = false;
            currentSpeed = 0f;
            return;
        }

        // Input reading (support both Input System and legacy fallback)
        float x = 0f;
        float z = 0f;
        bool sprintInput = false;
        bool jumpInput = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed) sprintInput = true;
            if (Keyboard.current.spaceKey.wasPressedThisFrame) jumpInput = true;
        }
        else
        {
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
            sprintInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            jumpInput = Input.GetKeyDown(KeyCode.Space);
        }

        // Camera-relative direction calculation
        Vector3 direction = Vector3.zero;
        if (cameraTransform != null)
        {
            Vector3 camFwd = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
            direction = (camFwd * z + camRight * x).normalized;
        }
        else
        {
            direction = new Vector3(x, 0f, z).normalized;
        }

        isMoving = direction.sqrMagnitude > 0.01f;
        isSprinting = isMoving && sprintInput;

        float targetSpeed = isSprinting ? sprintSpeed : speed;
        currentSpeed = isMoving ? targetSpeed : 0f;

        // Smooth character rotation towards move direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Ground check and jump
        isGrounded = controller != null && controller.isGrounded;
        if (isGrounded && verticalSpeed < 0f)
        {
            verticalSpeed = -2f;
        }

        if (jumpInput && isGrounded)
        {
            verticalSpeed = Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(gravity));
            if (ProceduralAudioManager.Instance != null)
            {
                ProceduralAudioManager.Instance.PlayJumpSound();
            }
        }

        verticalSpeed += gravity * Time.deltaTime;

        Vector3 move = direction * targetSpeed;
        move.y = verticalSpeed;

        if (controller != null)
        {
            controller.Move(move * Time.deltaTime);
        }
    }
}
