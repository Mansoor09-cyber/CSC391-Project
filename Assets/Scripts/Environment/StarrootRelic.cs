using UnityEngine;

public class StarrootRelic : MonoBehaviour, IInteractable
{
    [Header("Visual Effects")]
    public Light relicAuraLight;
    public float hoverSpeed = 1.8f;
    public float hoverHeight = 0.3f;
    public float spinSpeed = 45f;

    private Vector3 initialPos;
    private bool isClaimed = false;

    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        if (isClaimed)
        {
            // Ascend upwards with bright glow
            transform.position += Vector3.up * 1.5f * Time.deltaTime;
            transform.Rotate(Vector3.up, spinSpeed * 2.5f * Time.deltaTime, Space.World);
            if (relicAuraLight != null)
            {
                relicAuraLight.intensity = Mathf.PingPong(Time.time * 8f, 6f) + 4f;
            }
            return;
        }

        float yOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = initialPos + Vector3.up * yOffset;
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

        if (relicAuraLight != null)
        {
            relicAuraLight.intensity = 3.5f + Mathf.Sin(Time.time * 3f) * 1.2f;
        }
    }

    public string GetInteractionPrompt()
    {
        return "[E] Claim the Starroot Relic!";
    }

    public bool CanInteract()
    {
        return !isClaimed;
    }

    public void Interact(PlayerInteraction interactor)
    {
        if (isClaimed) return;
        isClaimed = true;

        if (ProceduralAudioManager.Instance != null)
        {
            ProceduralAudioManager.Instance.PlayRelicClaimedSound();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRelicCollected();
        }
    }
}
