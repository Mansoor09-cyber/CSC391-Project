using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardRotate : MonoBehaviour
{
    public float speed = 90f;

    void Update()
    {
        float direction = 0f;

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            direction = -1f;
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            direction = 1f;
        }

        transform.Rotate(0f, direction * speed * Time.deltaTime, 0f);
    }
}
