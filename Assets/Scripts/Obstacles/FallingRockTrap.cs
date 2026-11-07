using UnityEngine;

public class FallingRockTrap : MonoBehaviour
{
    [Header("Rock & Physics")]
    public Rigidbody rockRigidbody;
    public Transform rockSpawnPoint;
    public int damageAmount = 25;
    public float respawnDelay = 8f;
    public Vector3 initialForce = new Vector3(0f, 0f, 5f);

    private Vector3 initialPos;
    private Quaternion initialRot;
    private bool isTriggered = false;

    void Start()
    {
        if (rockRigidbody != null)
        {
            initialPos = rockRigidbody.transform.position;
            initialRot = rockRigidbody.transform.rotation;
            rockRigidbody.isKinematic = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            TriggerRock();
        }
    }

    public void TriggerRock()
    {
        if (rockRigidbody == null || isTriggered) return;
        isTriggered = true;

        rockRigidbody.isKinematic = false;
        rockRigidbody.AddForce(initialForce, ForceMode.Impulse);

        if (ProceduralAudioManager.Instance != null)
        {
            ProceduralAudioManager.Instance.PlayRockImpactSound();
        }

        Invoke(nameof(ResetRock), respawnDelay);
    }

    void ResetRock()
    {
        if (rockRigidbody != null)
        {
            rockRigidbody.linearVelocity = Vector3.zero;
            rockRigidbody.angularVelocity = Vector3.zero;
            rockRigidbody.isKinematic = true;
            rockRigidbody.transform.position = initialPos;
            rockRigidbody.transform.rotation = initialRot;
        }
        isTriggered = false;
    }
}
