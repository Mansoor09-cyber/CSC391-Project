
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float timeLeft = 30f;
    public int winScore = 3;

    public ScoreZone zone;
    public TMP_Text timerText;
    public GameObject endPanel;
    public TMP_Text endText;

    public KeyboardRotate cannonRotate;

    public bool gameOver = false;

    void Start()
    {
        if (endPanel != null)
            endPanel.SetActive(false);
    }

    void Update()
    {
        if (gameOver)
            return;

        timeLeft -= Time.deltaTime;
        timeLeft = Mathf.Max(timeLeft, 0f);

        if (timerText != null)
        {
            timerText.text =
                "Time: " + Mathf.CeilToInt(timeLeft);

            if (timeLeft <= 10f)
                timerText.color = Color.red;
        }

        if (zone != null && zone.knockedDown >= winScore)
        {
            EndGame("YOU WIN!");
        }
        else if (timeLeft <= 0f)
        {
            EndGame("YOU LOSE!");
        }
    }

    public void EndGame(string message)
    {
        if (gameOver)
            return;

        gameOver = true;

        if (cannonRotate != null)
            cannonRotate.enabled = false;

        if (endText != null)
            endText.text = message;

        if (endPanel != null)
            endPanel.SetActive(true);
    }
}
