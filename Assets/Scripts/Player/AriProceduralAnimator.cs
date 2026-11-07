using UnityEngine;

public class AriProceduralAnimator : MonoBehaviour
{
    [Header("Bone / Limb References")]
    public Transform head;
    public Transform torso;
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftLeg;
    public Transform rightLeg;
    public Transform backpack;

    [Header("Animation Tuning")]
    public float walkCycleSpeed = 8f;
    public float maxLegSwingAngle = 36f;
    public float maxArmSwingAngle = 40f;
    public float torsoBobAmount = 0.06f;
    public float breathingSpeed = 2f;
    public float breathingAmount = 0.015f;

    private PlayerMover mover;
    private CharacterController controller;
    private float cycleProgress = 0f;
    private Vector3 initialTorsoPos;
    private Quaternion initialLeftArmRot;
    private Quaternion initialRightArmRot;
    private Quaternion initialLeftLegRot;
    private Quaternion initialRightLegRot;
    private Quaternion initialHeadRot;

    void Start()
    {
        mover = GetComponent<PlayerMover>();
        controller = GetComponent<CharacterController>();

        if (torso != null) initialTorsoPos = torso.localPosition;
        if (leftArm != null) initialLeftArmRot = leftArm.localRotation;
        if (rightArm != null) initialRightArmRot = rightArm.localRotation;
        if (leftLeg != null) initialLeftLegRot = leftLeg.localRotation;
        if (rightLeg != null) initialRightLegRot = rightLeg.localRotation;
        if (head != null) initialHeadRot = head.localRotation;
    }

    void LateUpdate()
    {
        bool isGrounded = controller != null ? controller.isGrounded : true;
        bool isMoving = mover != null ? mover.isMoving : false;
        bool isSprinting = mover != null ? mover.isSprinting : false;
        float currentSpeed = mover != null ? mover.currentSpeed : 0f;

        if (!isGrounded)
        {
            // Jump pose
            AnimateJump();
        }
        else if (isMoving && currentSpeed > 0.1f)
        {
            // Locomotion (Walk / Run)
            float speedMult = isSprinting ? 1.4f : 1.0f;
            cycleProgress += Time.deltaTime * walkCycleSpeed * speedMult;
            AnimateLocomotion(isSprinting);
        }
        else
        {
            // Idle breathing
            AnimateIdle();
        }
    }

    void AnimateIdle()
    {
        float breath = Mathf.Sin(Time.time * breathingSpeed) * breathingAmount;
        if (torso != null)
        {
            torso.localPosition = Vector3.Lerp(torso.localPosition, initialTorsoPos + Vector3.up * breath, Time.deltaTime * 6f);
            torso.localRotation = Quaternion.Slerp(torso.localRotation, Quaternion.identity, Time.deltaTime * 6f);
        }

        // Return limbs gently to rest
        if (leftArm != null) leftArm.localRotation = Quaternion.Slerp(leftArm.localRotation, initialLeftArmRot, Time.deltaTime * 6f);
        if (rightArm != null) rightArm.localRotation = Quaternion.Slerp(rightArm.localRotation, initialRightArmRot, Time.deltaTime * 6f);
        if (leftLeg != null) leftLeg.localRotation = Quaternion.Slerp(leftLeg.localRotation, initialLeftLegRot, Time.deltaTime * 6f);
        if (rightLeg != null) rightLeg.localRotation = Quaternion.Slerp(rightLeg.localRotation, initialRightLegRot, Time.deltaTime * 6f);
        if (head != null) head.localRotation = Quaternion.Slerp(head.localRotation, initialHeadRot, Time.deltaTime * 6f);
    }

    void AnimateLocomotion(bool sprinting)
    {
        float legSwing = Mathf.Sin(cycleProgress) * maxLegSwingAngle * (sprinting ? 1.25f : 1.0f);
        float armSwing = Mathf.Sin(cycleProgress) * maxArmSwingAngle * (sprinting ? 1.35f : 1.0f);
        float bob = Mathf.Abs(Mathf.Sin(cycleProgress)) * torsoBobAmount * (sprinting ? 1.3f : 1.0f);
        float forwardLean = sprinting ? 12f : 4f;

        if (torso != null)
        {
            torso.localPosition = Vector3.Lerp(torso.localPosition, initialTorsoPos + Vector3.up * bob, Time.deltaTime * 15f);
            torso.localRotation = Quaternion.Slerp(torso.localRotation, Quaternion.Euler(forwardLean, 0f, 0f), Time.deltaTime * 10f);
        }

        // Legs alternate swing
        if (leftLeg != null) leftLeg.localRotation = Quaternion.Euler(legSwing, 0f, 0f);
        if (rightLeg != null) rightLeg.localRotation = Quaternion.Euler(-legSwing, 0f, 0f);

        // Arms oppose legs
        if (leftArm != null) leftArm.localRotation = Quaternion.Euler(-armSwing, 0f, 0f);
        if (rightArm != null) rightArm.localRotation = Quaternion.Euler(armSwing, 0f, 0f);

        if (head != null) head.localRotation = Quaternion.Slerp(head.localRotation, initialHeadRot, Time.deltaTime * 10f);
    }

    void AnimateJump()
    {
        // Limbs trail back/outward during jump
        if (leftLeg != null) leftLeg.localRotation = Quaternion.Slerp(leftLeg.localRotation, Quaternion.Euler(-25f, 0f, 0f), Time.deltaTime * 8f);
        if (rightLeg != null) rightLeg.localRotation = Quaternion.Slerp(rightLeg.localRotation, Quaternion.Euler(-20f, 0f, 0f), Time.deltaTime * 8f);
        if (leftArm != null) leftArm.localRotation = Quaternion.Slerp(leftArm.localRotation, Quaternion.Euler(35f, 0f, 20f), Time.deltaTime * 8f);
        if (rightArm != null) rightArm.localRotation = Quaternion.Slerp(rightArm.localRotation, Quaternion.Euler(35f, 0f, -20f), Time.deltaTime * 8f);
        if (torso != null) torso.localRotation = Quaternion.Slerp(torso.localRotation, Quaternion.Euler(-5f, 0f, 0f), Time.deltaTime * 8f);
    }
}
