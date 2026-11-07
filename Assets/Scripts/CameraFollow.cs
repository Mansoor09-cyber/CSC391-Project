
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0f, 5f, -7f);

    public float smoothing = 8f;
    public float rotationSmoothing = 8f;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothing * Time.deltaTime
        );

        Quaternion desiredRotation = Quaternion.LookRotation(
            target.position + Vector3.up * 1.5f - transform.position
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSmoothing * Time.deltaTime
        );
    }
}
