using UnityEngine;

public class CollapsingBridge : MonoBehaviour
{
    [Header("Collapse Settings")]
    public float collapseDelay = 0.65f;
    public float respawnDelay = 6.0f;
    public float fallSpeed = 6f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isCollapsing = false;
    private bool isFallen = false;
    private Collider plankCollider;
    private MeshRenderer plankRenderer;
    private float shakeTimer = 0f;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        plankCollider = GetComponent<Collider>();
        plankRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (isCollapsing && !isFallen)
        {
            shakeTimer += Time.deltaTime;
            // Wobble / shake plank
            float wobble = Mathf.Sin(shakeTimer * 45f) * 0.05f;
            transform.position = initialPosition + new Vector3(wobble, 0f, wobble);

            if (shakeTimer >= collapseDelay)
            {
                isFallen = true;
                if (plankCollider != null) plankCollider.enabled = false;
                if (ProceduralAudioManager.Instance != null)
                {
                    ProceduralAudioManager.Instance.PlayRockImpactSound();
                }
                Invoke(nameof(RespawnPlank), respawnDelay);
            }
        }
        else if (isFallen)
        {
            // Fall downward
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            if (plankRenderer != null)
            {
                plankRenderer.enabled = (transform.position.y > initialPosition.y - 8f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollapsing && !isFallen)
        {
            isCollapsing = true;
            shakeTimer = 0f;
        }
    }

    void RespawnPlank()
    {
        isCollapsing = false;
        isFallen = false;
        shakeTimer = 0f;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        if (plankCollider != null) plankCollider.enabled = true;
        if (plankRenderer != null) plankRenderer.enabled = true;
    }
}
