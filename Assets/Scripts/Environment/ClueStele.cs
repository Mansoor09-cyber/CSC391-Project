using UnityEngine;

public class ClueStele : MonoBehaviour, IInteractable
{
    [TextArea(2, 5)]
    public string clueMessage = "Ancient Inscription: 'Begin with the radiant SUN, seek the celestial STAR, pay tribute to the silver MOON, and honor the coiled SERPENT.'";

    public string GetInteractionPrompt()
    {
        return "[E] Read Ancient Stele Inscription";
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(PlayerInteraction interactor)
    {
        if (GameHUD.Instance != null)
        {
            GameHUD.Instance.ShowNotification(clueMessage, 5f);
        }

        if (ProceduralAudioManager.Instance != null)
        {
            ProceduralAudioManager.Instance.PlayPlateClickSound();
        }
    }
}
