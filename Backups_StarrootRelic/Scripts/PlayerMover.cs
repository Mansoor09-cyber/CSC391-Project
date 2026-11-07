
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = -20f;
    public GameManager manager;

    CharacterController controller;
    float verticalSpeed = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Stop movement when the game is over
        if (manager != null && manager.gameOver)
        {
            return;
        }

        float x = 0f;
        float z = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed)
            {
                x = -1f;
            }

            if (Keyboard.current.dKey.isPressed)
            {
                x = 1f;
            }

            if (Keyboard.current.sKey.isPressed)
            {
                z = -1f;
            }

            if (Keyboard.current.wKey.isPressed)
            {
                z = 1f;
            }
        }

        Vector3 direction = new Vector3(x, 0f, z).normalized;

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }

        if (controller.isGrounded && verticalSpeed < 0f)
        {
            verticalSpeed = -2f;
        }

        verticalSpeed += gravity * Time.deltaTime;

        Vector3 move = direction * speed;
        move.y = verticalSpeed;

        controller.Move(move * Time.deltaTime);
    }
}
