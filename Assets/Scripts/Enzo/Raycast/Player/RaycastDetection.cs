using UnityEngine;

public class RaycastDetection : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private Transform raycastLeftGround;
    [SerializeField] private Transform raycastRightGround;
    
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private LayerMask layerWall;
    
    [SerializeField] private float distanceToDetectFloor = 0.2f;
    [SerializeField] private float distanceToDetectWall = 0.3f;

    private RaycastHit2D hitLeftGround;
    private RaycastHit2D hitRightGround;
    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;
    
    [Header("Ne pas set up")]
    public bool isGrounded;
    public bool stopRight;
    public bool stopLeft;

    private void Update()
    {
        SetRaycast();
        SetBool();
    }

    bool isRaycastNotNull(RaycastHit2D raycastHit2D)
    {
        if (raycastHit2D.collider != null)
        {
            return true;
        }
        return false;
    }
   
    bool isRaycastOfListIsNotNull(RaycastHit2D [] raycastsHit2D)
    {
        foreach (var raycast in raycastsHit2D)
        {
            if (raycast.collider != null)
            {
                return true;
            }
        }
        return false;
    }
    
    private void SetRaycast()
    {
        hitLeftGround = Physics2D.Raycast(raycastLeftGround.position, Vector2.down, distanceToDetectFloor, layerGround);
        hitRightGround = Physics2D.Raycast(raycastRightGround.position, Vector2.down,distanceToDetectFloor, layerGround);
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceToDetectWall, layerWall);
        hitRight = Physics2D.Raycast(transform.position, Vector2.right, distanceToDetectWall, layerWall);
    }

    private void SetBool()
    {
        isGrounded = isRaycastOfListIsNotNull(new RaycastHit2D[] {hitLeftGround, hitRightGround});
        stopLeft = isRaycastNotNull((hitLeft));
        stopRight = isRaycastNotNull((hitRight));
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastLeftGround.position, raycastLeftGround.position + Vector3.down * distanceToDetectFloor);
        Gizmos.DrawLine(raycastRightGround.position, raycastRightGround.position + Vector3.down * distanceToDetectFloor);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * distanceToDetectWall);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * distanceToDetectWall);
    }
}
