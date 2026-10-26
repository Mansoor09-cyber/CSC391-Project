using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyMover : MonoBehaviour
{
    public float moveForce = 40f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float x = 0f;
        float z = 0f;
        if (Keyboard.current.aKey.isPressed) { x = -1f; }
        if (Keyboard.current.dKey.isPressed) { x = 1f; }
        if (Keyboard.current.sKey.isPressed) { z = -1f; }
        if (Keyboard.current.wKey.isPressed) { z = 1f; }

        Vector3 direction = new Vector3(x, 0f, z).normalized;
        rb.AddForce(direction * moveForce);
    }
}