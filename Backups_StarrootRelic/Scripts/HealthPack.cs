using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healAmount = 25;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().Heal(healAmount);
            Destroy(gameObject);
        }
    }
}