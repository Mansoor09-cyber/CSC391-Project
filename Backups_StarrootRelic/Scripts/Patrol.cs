using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Vector3 direction = new Vector3(1f, 0f, 0f);
    public float distance = 10f;
    public float speed = 2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPos + direction * t;
    }
}