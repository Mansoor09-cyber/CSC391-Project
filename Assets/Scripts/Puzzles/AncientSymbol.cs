using UnityEngine;

public class AncientSymbol : MonoBehaviour, IInteractable
{
    [Header("Symbol Settings")]
    public int symbolIndex = 0; // 0=Sun, 1=Star, 2=Moon, 3=Serpent
    public string symbolName = "Sun";
    public SymbolPuzzle puzzleController;
    public Light runeLight;
    public MeshRenderer symbolRenderer;

    [Header("Visual Feedback")]
    public Color activeColor = new Color(0.9f, 0.7f, 0.1f);
    public Color inactiveColor = new Color(0.2f, 0.2f, 0.2f);

    public bool isActivated { get; private set; }

    void Start()
    {
        ResetSymbol();
    }

    public string GetInteractionPrompt()
    {
        return "[E] Inscribe the " + symbolName + " Rune";
    }

    public bool CanInteract()
    {
        return !isActivated && (puzzleController == null || !puzzleController.isSolved);
    }

    public void Interact(PlayerInteraction interactor)
    {
        if (isActivated) return;

        isActivated = true;
        SetVisuals(true);

        if (ProceduralAudioManager.Instance != null)
        {
            ProceduralAudioManager.Instance.PlayPlateClickSound();
        }

        if (puzzleController != null)
        {
            puzzleController.OnSymbolInteracted(this);
        }
    }

    public void SetVisuals(bool active)
    {
        if (runeLight != null)
        {
            runeLight.color = active ? activeColor : inactiveColor;
            runeLight.intensity = active ? 3f : 0.4f;
        }
    }

    public void ResetSymbol()
    {
        isActivated = false;
        SetVisuals(false);
    }
}
