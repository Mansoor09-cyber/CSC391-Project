using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRadius = 2.8f;
    public LayerMask interactableLayer = ~0;
    
    private IInteractable currentTarget;

    void Update()
    {
        FindNearestInteractable();

        bool interactPressed = false;
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            interactPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            interactPressed = true;
        }

        if (interactPressed && currentTarget != null && currentTarget.CanInteract())
        {
            currentTarget.Interact(this);
            // Recheck after interaction
            FindNearestInteractable();
        }
    }

    void FindNearestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius, interactableLayer, QueryTriggerInteraction.Collide);
        IInteractable closest = null;
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable == null)
            {
                interactable = hit.GetComponentInParent<IInteractable>();
            }

            if (interactable != null && interactable.CanInteract())
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = interactable;
                }
            }
        }

        currentTarget = closest;

        if (GameHUD.Instance != null)
        {
            if (currentTarget != null)
            {
                GameHUD.Instance.ShowInteractionPrompt(currentTarget.GetInteractionPrompt());
            }
            else
            {
                GameHUD.Instance.HideInteractionPrompt();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
