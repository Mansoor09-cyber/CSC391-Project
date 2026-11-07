using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Original Preserved Fields")]
    public float timeLeft = 600f;
    public int winScore = 3;
    public ScoreZone zone;
    public TMP_Text timerText;
    public GameObject endPanel;
    public TMP_Text endText;
    public KeyboardRotate cannonRotate;
    public bool gameOver = false;

    [Header("The Starroot Relic Systems")]
    public int crystalsCollected = 0;
    public int totalCrystals = 3;
    public bool pressurePlatesSolved = false;
    public bool symbolsSolved = false;
    public bool templeGateUnlocked = false;
    public bool relicClaimed = false;

    [Header("Temple & Gate References")]
    public AncientGate finalTempleGate;
    public GameHUD hud;
    public Health playerHealth;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (endPanel != null)
        {
            endPanel.SetActive(false);
        }

        UpdateObjective();
        UpdateCrystalHUD();

        if (hud == null)
        {
            hud = FindFirstObjectByType<GameHUD>();
        }

        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<Health>();
        }
    }

    void Update()
    {
        if (gameOver)
        {
            bool restartPressed = false;
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                restartPressed = true;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                restartPressed = true;
            }

            if (restartPressed)
            {
                RestartGame();
            }
            return;
        }

        // Optional countdown
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            timeLeft = Mathf.Max(timeLeft, 0f);

            if (timerText != null)
            {
                timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
                if (timeLeft <= 10f)
                {
                    timerText.color = Color.red;
                }
            }
        }
    }

    public void CollectCrystal(StarrootCrystal crystal)
    {
        crystalsCollected++;
        UpdateCrystalHUD();

        if (hud != null)
        {
            hud.ShowNotification("Discovered " + crystal.crystalName + "! (" + crystalsCollected + "/" + totalCrystals + ")");
        }

        CheckTempleGateUnlock();
        UpdateObjective();
    }

    public void OnPressurePlatesSolved()
    {
        pressurePlatesSolved = true;
        CheckTempleGateUnlock();
        UpdateObjective();
    }

    public void OnSymbolsSolved()
    {
        symbolsSolved = true;
        CheckTempleGateUnlock();
        UpdateObjective();
    }

    void CheckTempleGateUnlock()
    {
        if (crystalsCollected >= totalCrystals && !templeGateUnlocked)
        {
            templeGateUnlocked = true;
            if (finalTempleGate != null)
            {
                finalTempleGate.OpenGate();
            }

            if (hud != null)
            {
                hud.ShowNotification("All 3 Starroot Crystals assembled! The Final Temple Gate has opened!", 5f);
            }
        }
    }

    void UpdateObjective()
    {
        if (hud == null) return;

        if (relicClaimed)
        {
            hud.SetObjective("Victory! Starroot Relic secured!");
        }
        else if (templeGateUnlocked)
        {
            hud.SetObjective("Objective: Enter the Starroot Sanctum and claim the Relic!");
        }
        else if (!pressurePlatesSolved)
        {
            hud.SetObjective("Objective 1: Explore the jungle and activate the Ancient Pressure Plates.");
        }
        else if (!symbolsSolved)
        {
            hud.SetObjective("Objective 2: Follow the Celestial Clue to solve the Ancient Symbol Runes.");
        }
        else if (crystalsCollected < totalCrystals)
        {
            hud.SetObjective("Objective 3: Find all 3 hidden Starroot Crystals (" + crystalsCollected + "/" + totalCrystals + ")");
        }
    }

    void UpdateCrystalHUD()
    {
        if (hud != null)
        {
            hud.SetCrystalCounter(crystalsCollected, totalCrystals);
        }
    }

    public void OnRelicCollected()
    {
        relicClaimed = true;
        EndGame("STARROOT RELIC FOUND!", true);
    }

    public void EndGame(string message)
    {
        EndGame(message, message.Contains("WIN") || message.Contains("FOUND") || message.Contains("STARROOT"));
    }

    public void EndGame(string message, bool isVictory)
    {
        if (gameOver) return;
        gameOver = true;

        if (cannonRotate != null)
        {
            cannonRotate.enabled = false;
        }

        if (endText != null)
        {
            endText.text = message + "\n\nPress R to Restart";
        }

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        if (hud != null)
        {
            if (isVictory)
            {
                hud.ShowVictory();
            }
            else
            {
                hud.ShowGameOver();
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
