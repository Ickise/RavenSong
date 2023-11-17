using UnityEngine;

public class SphereCastDetection : MonoBehaviour
{
    [Header("Ne pas set up")]
    public RaycastHit2D hitWall;
    public RaycastHit2D hitGround;
    public RaycastHit2D hitPlayer;
    
    public bool isTouchingWall;
    
    [Header("À set up")]
    [SerializeField] private float distanceToDetect = 0.2f;
    [SerializeField] private float distanceToDetectGround = 0.12f;
    
    [SerializeField] private LayerMask[] listOfLayer;
    
    private void FixedUpdate()
    { 
        hitWall = Physics2D.CircleCast(transform.position, distanceToDetect, Vector2.up,0.5f, listOfLayer[0] );
        hitGround = Physics2D.Raycast(transform.position, Vector2.down, distanceToDetectGround, listOfLayer[1]);

        if (hitWall || hitGround || isTouchingWall)
        {
            hitPlayer = Physics2D.CircleCast(transform.position, distanceToDetect, Vector2.up,0.5f, listOfLayer[2]);
            isTouchingWall = true;
        }
    }
    
    bool isRaycastNotNull(RaycastHit2D raycastHit2D)
    {
        if (raycastHit2D.collider != null)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distanceToDetectGround);
        Gizmos.DrawSphere(transform.position,distanceToDetect);
    }
}
