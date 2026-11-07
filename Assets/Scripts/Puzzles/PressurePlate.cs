using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Plate Properties")]
    public int plateIndex = 0; // 0, 1, 2
    public PressurePlatePuzzle puzzleController;
    public Transform plateMeshTransform;
    public Light runeLight;
    public MeshRenderer plateRenderer;

    [Header("Visual Feedback")]
    public float depressDistance = 0.08f;
    public Color inactiveColor = new Color(0.3f, 0.3f, 0.3f);
    public Color activeColor = new Color(0.2f, 1f, 0.4f);

    private Vector3 initialPos;
    private bool isPressed = false;
    private bool isLocked = false;

    void Start()
    {
        if (plateMeshTransform != null)
        {
            initialPos = plateMeshTransform.localPosition;
        }
        else
        {
            initialPos = transform.localPosition;
        }

        SetVisuals(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isLocked) return;

        if (other.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            Depress(true);
            SetVisuals(true);

            if (ProceduralAudioManager.Instance != null)
            {
                ProceduralAudioManager.Instance.PlayPlateClickSound();
            }

            if (puzzleController != null)
            {
                puzzleController.OnPlateStepped(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isLocked) return;

        if (other.CompareTag("Player") && isPressed)
        {
            isPressed = false;
            Depress(false);
            if (puzzleController != null && !puzzleController.isSolved)
            {
                // Return to unpressed unless locked by puzzle
                SetVisuals(false);
            }
        }
    }

    void Depress(bool down)
    {
        Transform target = plateMeshTransform != null ? plateMeshTransform : transform;
        Vector3 targetPos = down ? initialPos - Vector3.up * depressDistance : initialPos;
        target.localPosition = targetPos;
    }

    public void SetVisuals(bool active)
    {
        if (runeLight != null)
        {
            runeLight.color = active ? activeColor : inactiveColor;
            runeLight.intensity = active ? 2.5f : 0.5f;
        }
    }

    public void ResetPlate()
    {
        isPressed = false;
        isLocked = false;
        Depress(false);
        SetVisuals(false);
    }

    public void LockPlate()
    {
        isPressed = true;
        isLocked = true;
        Depress(true);
        SetVisuals(true);
    }
}
