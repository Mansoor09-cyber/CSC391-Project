using UnityEngine;

public class AncientGate : MonoBehaviour
{
    [Header("Door Transforms")]
    public Transform leftDoor;
    public Transform rightDoor;
    public Collider blockingCollider;

    [Header("Movement Settings")]
    public Vector3 leftDoorOpenOffset = new Vector3(-3.2f, 0f, 0f);
    public Vector3 rightDoorOpenOffset = new Vector3(3.2f, 0f, 0f);
    public float openSpeed = 1.2f;

    [Header("State")]
    public bool isOpen = false;

    private Vector3 leftInitialPos;
    private Vector3 rightInitialPos;
    private float openProgress = 0f;
    private bool isOpening = false;

    void Start()
    {
        if (leftDoor != null) leftInitialPos = leftDoor.localPosition;
        if (rightDoor != null) rightInitialPos = rightDoor.localPosition;
    }

    void Update()
    {
        if (isOpening)
        {
            openProgress += Time.deltaTime * openSpeed;
            float smoothT = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(openProgress));

            if (leftDoor != null)
            {
                leftDoor.localPosition = Vector3.Lerp(leftInitialPos, leftInitialPos + leftDoorOpenOffset, smoothT);
            }

            if (rightDoor != null)
            {
                rightDoor.localPosition = Vector3.Lerp(rightInitialPos, rightInitialPos + rightDoorOpenOffset, smoothT);
            }

            if (openProgress >= 1f)
            {
                isOpening = false;
                isOpen = true;
                if (blockingCollider != null)
                {
                    blockingCollider.enabled = false;
                }
            }
        }
    }

    public void OpenGate()
    {
        if (isOpen || isOpening) return;
        isOpening = true;
        openProgress = 0f;

        if (blockingCollider != null)
        {
            blockingCollider.enabled = false;
        }

        if (ProceduralAudioManager.Instance != null)
        {
            ProceduralAudioManager.Instance.PlayGateGrindSound();
        }
    }
}
