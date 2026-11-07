using UnityEngine;

public class SymbolPuzzle : MonoBehaviour
{
    [Header("Symbols & Sequence")]
    public AncientSymbol[] symbols;
    public int[] targetSequence = new int[] { 0, 1, 2, 3 }; // Sun -> Star -> Moon -> Serpent
    public AncientGate linkedGate;
    public string clueText = "Follow the Celestial Cycle: Sun -> Star -> Moon -> Serpent";

    [Header("State")]
    public bool isSolved = false;
    private int currentStepIndex = 0;

    public void OnSymbolInteracted(AncientSymbol symbol)
    {
        if (isSolved) return;

        int expectedSymbol = targetSequence[currentStepIndex];

        if (symbol.symbolIndex == expectedSymbol)
        {
            currentStepIndex++;

            if (GameHUD.Instance != null)
            {
                GameHUD.Instance.ShowNotification(symbol.symbolName + " Rune Aligned! (" + currentStepIndex + "/" + targetSequence.Length + ")");
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
                    GameHUD.Instance.ShowNotification("Symbol Puzzle Solved! Ancient Pathway Unlocked!");
                }

                if (linkedGate != null)
                {
                    linkedGate.OpenGate();
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnSymbolsSolved();
                }
            }
        }
        else
        {
            // Wrong symbol
            if (ProceduralAudioManager.Instance != null)
            {
                ProceduralAudioManager.Instance.PlayPuzzleErrorSound();
            }

            if (GameHUD.Instance != null)
            {
                GameHUD.Instance.ShowNotification("The runes reject your sequence! Resetting...");
            }

            ResetAllSymbols();
        }
    }

    public void ResetAllSymbols()
    {
        currentStepIndex = 0;
        foreach (var s in symbols)
        {
            if (s != null) s.ResetSymbol();
        }
    }
}
