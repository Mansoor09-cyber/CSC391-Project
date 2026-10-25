
using UnityEngine;
using TMPro;

public class ScoreZone : MonoBehaviour
{
    public int knockedDown = 0;
    public TMP_Text scoreText;
    public GameManager manager;

    void Start()
    {
        UpdateText();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            knockedDown++;

            // Add 2 bonus seconds for each target
            if (manager != null && !manager.gameOver)
            {
                manager.timeLeft += 2f;
            }

            UpdateText();
            Destroy(other.gameObject, 1f);
        }
    }

    void UpdateText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + knockedDown;
        }
    }
}
