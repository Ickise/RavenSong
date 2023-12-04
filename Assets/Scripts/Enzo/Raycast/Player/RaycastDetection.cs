using UnityEngine;

public class RaycastDetection : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private Transform raycastLeftGround;
    [SerializeField] private Transform raycastRightGround;
    [SerializeField] private Transform raycastLeftUp;
    [SerializeField] private Transform raycastRightUp;
    [SerializeField] private Transform raycastLeftDown;
    [SerializeField] private Transform raycastRightDown;
    
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private LayerMask layerWall;
    
    [SerializeField] private float distanceToDetectFloor = 0.2f;
    [SerializeField] private float distanceToDetectWall = 0.3f;

    private RaycastHit2D hitLeftGround;
    private RaycastHit2D hitRightGround;
    private RaycastHit2D hitLeftUp;
    private RaycastHit2D hitRightUp;
    private RaycastHit2D hitLeftDown;
    private RaycastHit2D hitRightDown;
    
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
        hitLeftUp = Physics2D.Raycast(raycastLeftUp.position, Vector2.left, distanceToDetectWall, layerWall);
        hitRightUp = Physics2D.Raycast(raycastRightUp.position, Vector2.right, distanceToDetectWall, layerWall);
        hitLeftDown = Physics2D.Raycast(raycastLeftDown.position, Vector2.left, distanceToDetectWall, layerWall);
        hitRightDown = Physics2D.Raycast(raycastRightDown.position, Vector2.right, distanceToDetectWall, layerWall);
    }

    private void SetBool()
    {
        isGrounded = isRaycastOfListIsNotNull(new RaycastHit2D[] {hitLeftGround, hitRightGround});
        stopLeft = isRaycastNotNull((hitLeftUp));
        stopRight = isRaycastNotNull((hitRightUp));
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastLeftGround.position, raycastLeftGround.position + Vector3.down * distanceToDetectFloor);
        Gizmos.DrawLine(raycastRightGround.position, raycastRightGround.position + Vector3.down * distanceToDetectFloor);
        Gizmos.DrawLine(raycastLeftUp.position, raycastLeftUp.position + Vector3.left * distanceToDetectWall);
        Gizmos.DrawLine(raycastRightUp.position, raycastRightUp.position + Vector3.right * distanceToDetectWall);
        Gizmos.DrawLine(raycastLeftDown.position, raycastLeftDown.position + Vector3.left * distanceToDetectWall);
        Gizmos.DrawLine(raycastRightDown.position, raycastRightDown.position + Vector3.right * distanceToDetectWall);
    }
}
