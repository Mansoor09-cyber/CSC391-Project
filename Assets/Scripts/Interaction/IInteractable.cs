using UnityEngine;

public interface IInteractable
{
    string GetInteractionPrompt();
    void Interact(PlayerInteraction interactor);
    bool CanInteract();
}
