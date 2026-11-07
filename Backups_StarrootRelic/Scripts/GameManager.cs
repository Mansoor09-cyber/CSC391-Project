using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
        {
            endPanel.SetActive(false);
        }

        if (timerText != null)
        {
            timerText.color = Color.white;
        }
    }

    void Update()
    {
        // Restart when R is pressed after the game ends
        if (gameOver)
        {
            if (Keyboard.current != null &&
                Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().buildIndex
                );
            }

            return;
        }

        // Countdown timer
        timeLeft -= Time.deltaTime;
        timeLeft = Mathf.Max(timeLeft, 0f);

        if (timerText != null)
        {
            timerText.text =
                "Time: " + Mathf.CeilToInt(timeLeft);

            if (timeLeft <= 10f)
            {
                timerText.color = Color.red;
            }
        }

        // Win or lose conditions
        if (zone != null && zone.knockedDown >= winScore)
        {
            EndGame("YOU WIN!");
        }
        else if (timeLeft <= 0f)
        {
            EndGame("YOU LOSE!");
        }
    }

    public void AddBonusTime(float seconds)
    {
        if (!gameOver)
        {
            timeLeft += seconds;
        }
    }

    public void EndGame(string message)
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;

        // Stop aiming after the game ends
        if (cannonRotate != null)
        {
            cannonRotate.enabled = false;
        }

        if (endText != null)
        {
            endText.text = message + "\nPress R to Restart";
        }

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }
    }
}
