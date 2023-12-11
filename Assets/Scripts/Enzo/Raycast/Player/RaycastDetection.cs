using UnityEngine;

public class RaycastDetection : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private Transform capsuleCastRight;
    [SerializeField] private Transform capsuleCastLeft;
    [SerializeField] private Transform capsuleCastGround;
    
    [SerializeField] private Vector2 capsuleSizeWall = new Vector2(0.1f, 2f); 
    [SerializeField] private Vector2 capsuleSizeGround = new Vector2(1f, 0.05f);
    
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private LayerMask layerWall;
    
    [SerializeField] private float distanceToDetectFloor = 0f;
    [SerializeField] private float distanceToDetectWall = 0f;
    [SerializeField] private float capsuleAngle = 0f;

    [Header("Ne pas set up")]
    public bool isGrounded;
    public bool stopRight;
    public bool stopLeft;
    
    private RaycastHit2D capsuleCastRightHit;
    private RaycastHit2D capsuleCastLeftHit;
    private RaycastHit2D capsuleCastGroundHit;
    private void Update()
    {
        SetRaycast();
        SetBool();
    }

    private void SetRaycast()
    {
        capsuleCastRightHit = Physics2D.CapsuleCast(capsuleCastRight.position, capsuleSizeWall, CapsuleDirection2D.Vertical,
            capsuleAngle, Vector2.right, distanceToDetectWall, layerWall);
        
        capsuleCastLeftHit = Physics2D.CapsuleCast(capsuleCastLeft.position, capsuleSizeWall, CapsuleDirection2D.Vertical,
            capsuleAngle, Vector2.right, distanceToDetectWall, layerWall);
        
        capsuleCastGroundHit = Physics2D.CapsuleCast(capsuleCastGround.position, capsuleSizeGround, CapsuleDirection2D.Horizontal,
            capsuleAngle, Vector2.down, distanceToDetectFloor, layerGround);
    }

    private void SetBool()
    { 
        isGrounded = capsuleCastGroundHit;

        stopLeft = capsuleCastLeftHit;
        stopRight = capsuleCastRightHit;
    }
}
