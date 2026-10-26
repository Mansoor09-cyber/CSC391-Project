using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 8f, -7f);
    public float smoothing = 5f;

    void LateUpdate()
    {
        Vector3 wanted = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position, wanted, smoothing * Time.deltaTime);
        transform.LookAt(target);
    }
}
