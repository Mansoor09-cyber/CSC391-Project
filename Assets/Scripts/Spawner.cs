
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public GameManager manager;
    public TMP_Text shotsText;

    public int shots = 0;
    public int maxShots = 8;

    void Start()
    {
        UpdateShotsText();
    }

    void Update()
    {
        if (manager == null || manager.gameOver)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (shots >= maxShots)
            {
                Debug.Log("No shots remaining!");
                return;
            }

            Instantiate(prefab, transform.position, transform.rotation);

            shots++;
            UpdateShotsText();
        }
    }

    void UpdateShotsText()
    {
        if (shotsText != null)
        {
            shotsText.text = "Shots: " + shots + " / " + maxShots;
        }
    }
}
