using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance { get; private set; }

    [Header("HUD Text Elements")]
    public TMP_Text objectiveText;
    public TMP_Text crystalCounterText;
    public TMP_Text interactionPromptText;
    public TMP_Text notificationBannerText;

    [Header("Panels")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public GameObject victoryPanel;
    public TMP_Text victoryText;

    private float notificationTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (interactionPromptText != null) interactionPromptText.gameObject.SetActive(false);
        if (notificationBannerText != null) notificationBannerText.text = "";
    }

    void Update()
    {
        if (notificationTimer > 0f)
        {
            notificationTimer -= Time.deltaTime;
            if (notificationTimer <= 0f && notificationBannerText != null)
            {
                notificationBannerText.text = "";
            }
        }
    }

    public void SetObjective(string message)
    {
        if (objectiveText != null)
        {
            objectiveText.text = message;
        }
    }

    public void SetCrystalCounter(int current, int total)
    {
        if (crystalCounterText != null)
        {
            crystalCounterText.text = "Starroot Crystals: " + current + " / " + total;
        }
    }

    public void ShowInteractionPrompt(string prompt)
    {
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(true);
            interactionPromptText.text = prompt;
        }
    }

    public void HideInteractionPrompt()
    {
        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }

    public void ShowNotification(string message, float duration = 3.5f)
    {
        if (notificationBannerText != null)
        {
            notificationBannerText.text = message;
            notificationTimer = duration;
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (gameOverText != null)
            {
                gameOverText.text = "YOU DIED\n\nPress R to Restart";
            }
        }
    }

    public void ShowVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            if (victoryText != null)
            {
                victoryText.text = "STARROOT RELIC FOUND!\n\nYou restored the ancient balance of the Jungle!\n\nPress R to Play Again";
            }
        }
    }
}
