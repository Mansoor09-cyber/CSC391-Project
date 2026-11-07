using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 20;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
}