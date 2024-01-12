using UnityEngine;

public class RaycastDetection : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private Transform capsuleCastRight;
    [SerializeField] private Transform capsuleCastLeft;
    [SerializeField] private Transform capsuleCastGround;

    // [SerializeField] private Vector2 capsuleSizeWall = new Vector2(0.1f, 2f);
    [SerializeField] private Vector2 capsuleSizeGround = new Vector2(1f, 0.1f);

    [SerializeField] private LayerMask layerGround;
    [SerializeField] private LayerMask layerWall;

    [SerializeField] private float distanceToDetectFloor = 0f;
    [SerializeField] private float distanceToDetectWall = 0f;
    [SerializeField] private float capsuleAngle = 0f;

    [Header("Ne pas set up")]
    public bool isGrounded;
    public bool stopRight;
    public bool stopLeft;

    // private RaycastHit2D capsuleCastHit;
    // private RaycastHit2D capsuleCastLeftHit;
    private RaycastHit2D capsuleCastGroundHit;
    private void Update()
    {
        SetRaycast();
        SetBool();
    }

    private void SetRaycast()
    {
        // capsuleCastRightHit = Physics2D.CapsuleCast(capsuleCastRight.position, capsuleSizeWall, CapsuleDirection2D.Vertical,
        //     capsuleAngle, Vector2.right, distanceToDetectWall, layerWall);

        // capsuleCastLeftHit = Physics2D.CapsuleCast(capsuleCastLeft.position, capsuleSizeWall, CapsuleDirection2D.Vertical,
        //     capsuleAngle, Vector2.right, distanceToDetectWall, layerWall);

        capsuleCastGroundHit = Physics2D.CapsuleCast(capsuleCastGround.position, capsuleSizeGround, CapsuleDirection2D.Horizontal,
            capsuleAngle, Vector2.down, distanceToDetectFloor, layerGround);
    }

    public RaycastHit2D RaycastOnRoll(float directionX)
    {
        return Physics2D.CapsuleCast(transform.position, new Vector2(0.1f, transform.localScale.y - 0.1f), CapsuleDirection2D.Vertical,
            capsuleAngle, Vector2.right * directionX, distanceToDetectWall, layerWall);
    }

    public RaycastHit2D RaycastJump
    {
        get
        {
            return Physics2D.CapsuleCast(transform.position - capsuleCastGround.localPosition, capsuleSizeGround, CapsuleDirection2D.Horizontal,
            capsuleAngle, Vector2.down, distanceToDetectFloor, layerGround);
        }
    }

    private void SetBool()
    {
        isGrounded = capsuleCastGroundHit;

        // stopLeft = capsuleCastLeftHit;
        // stopRight = capsuleCastHit;
    }
}
