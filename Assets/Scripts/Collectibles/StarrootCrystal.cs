using UnityEngine;

public class StarrootCrystal : MonoBehaviour, IInteractable
{
    [Header("Crystal Settings")]
    public int crystalIndex = 0; // 0, 1, 2
    public string crystalName = "Emerald Starroot Crystal";
    public Color crystalColor = Color.green;
    public float bobSpeed = 2.5f;
    public float bobHeight = 0.25f;
    public float rotateSpeed = 65f;
    
    [Header("Visual Components")]
    public Light crystalLight;
    public MeshRenderer crystalRenderer;

    private Vector3 initialPosition;
    private bool collected = false;

    void Start()
    {
        initialPosition = transform.position;
        if (crystalLight != null)
        {
            crystalLight.color = crystalColor;
        }
    }

    void Update()
    {
        if (collected) return;

        // Hover bobbing
        float newY = initialPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Continuous rotation
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

        // Pulse light
        if (crystalLight != null)
        {
            crystalLight.intensity = 2f + Mathf.Sin(Time.time * 3f) * 0.8f;
        }
    }

    public string GetInteractionPrompt()
    {
        return "[E] Collect " + crystalName;
    }

    public bool CanInteract()
    {
        return !collected;
    }

    public void Interact(PlayerInteraction interactor)
    {
        Collect();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            Collect();
        }
    }

    void Collect()
    {
        if (collected) return;
        collected = true;

        if (ProceduralAudioManager.Instance != null)
        {
            ProceduralAudioManager.Instance.PlayCrystalPickupSound();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectCrystal(this);
        }

        // Hide mesh & collider
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        if (crystalRenderer != null) crystalRenderer.enabled = false;
        if (crystalLight != null) crystalLight.enabled = false;

        Destroy(gameObject, 0.5f);
    }
}
