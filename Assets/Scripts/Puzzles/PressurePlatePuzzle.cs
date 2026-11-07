using UnityEngine;

public class PressurePlatePuzzle : MonoBehaviour
{
    [Header("Plates & Sequence")]
    public PressurePlate[] plates;
    public int[] targetSequence = new int[] { 0, 1, 2 };
    public AncientGate linkedGate;

    [Header("State")]
    public bool isSolved = false;
    private int currentStepIndex = 0;

    public void OnPlateStepped(PressurePlate plate)
    {
        if (isSolved) return;

        int expectedPlate = targetSequence[currentStepIndex];

        if (plate.plateIndex == expectedPlate)
        {
            // Correct plate!
            currentStepIndex++;
            plate.LockPlate();

            if (GameHUD.Instance != null)
            {
                GameHUD.Instance.ShowNotification("Ancient Plate Activated (" + currentStepIndex + "/" + targetSequence.Length + ")");
            }

            if (currentStepIndex >= targetSequence.Length)
            {
                // Solved!
                isSolved = true;
                if (ProceduralAudioManager.Instance != null)
                {
                    ProceduralAudioManager.Instance.PlayPuzzleSolvedSound();
                }

                if (GameHUD.Instance != null)
                {
                    GameHUD.Instance.ShowNotification("Pressure Plate Puzzle Solved! Ancient Gate Unlocked!");
                }

                if (linkedGate != null)
                {
                    linkedGate.OpenGate();
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnPressurePlatesSolved();
                }
            }
        }
        else
        {
            // Incorrect plate - Reset puzzle
            if (ProceduralAudioManager.Instance != null)
            {
                ProceduralAudioManager.Instance.PlayPuzzleErrorSound();
            }

            if (GameHUD.Instance != null)
            {
                GameHUD.Instance.ShowNotification("Incorrect sequence! The ancient plates reset.");
            }

            ResetAllPlates();
        }
    }

    public void ResetAllPlates()
    {
        currentStepIndex = 0;
        foreach (var p in plates)
        {
            if (p != null) p.ResetPlate();
        }
    }
}
