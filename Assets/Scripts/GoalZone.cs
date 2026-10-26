using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public GameManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.EndGame("YOU WIN!");
        }
    }
}