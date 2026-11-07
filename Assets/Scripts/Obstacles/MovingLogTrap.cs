using UnityEngine;

public class MovingLogTrap : MonoBehaviour
{
    [Header("Motion Settings")]
    public Vector3 moveAxis = new Vector3(1f, 0f, 0f);
    public float distance = 5f;
    public float speed = 2.5f;
    public bool isRotating = false;
    public float rotateSpeed = 90f;

    [Header("Damage")]
    public int damageAmount = 20;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            float offset = Mathf.PingPong(Time.time * speed, distance) - (distance * 0.5f);
            transform.position = startPos + moveAxis.normalized * offset;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
