using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float launchForce = 20f;
    public float lifeTime = 5f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * launchForce, ForceMode.Impulse);
        Destroy(gameObject, lifeTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Hit a target!");
        }
    }
}