using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Spike Mechanism")]
    public Transform spikesMeshTransform;
    public float loweredY = -0.3f;
    public float raisedY = 0.5f;
    public float downDuration = 2.2f;
    public float upDuration = 1.4f;
    public int damageAmount = 25;

    [Header("State")]
    public bool isRaised = false;

    private float timer = 0f;
    private Vector3 initialPos;

    void Start()
    {
        if (spikesMeshTransform != null)
        {
            initialPos = spikesMeshTransform.localPosition;
            spikesMeshTransform.localPosition = new Vector3(initialPos.x, loweredY, initialPos.z);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        float cycle = isRaised ? upDuration : downDuration;

        if (timer >= cycle)
        {
            timer = 0f;
            isRaised = !isRaised;

            if (isRaised && ProceduralAudioManager.Instance != null)
            {
                ProceduralAudioManager.Instance.PlayPlateClickSound();
            }
        }

        if (spikesMeshTransform != null)
        {
            float targetY = isRaised ? raisedY : loweredY;
            Vector3 targetPos = new Vector3(initialPos.x, targetY, initialPos.z);
            spikesMeshTransform.localPosition = Vector3.Lerp(spikesMeshTransform.localPosition, targetPos, Time.deltaTime * 12f);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (isRaised && other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
