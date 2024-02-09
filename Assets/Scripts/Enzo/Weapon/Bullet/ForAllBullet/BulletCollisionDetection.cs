using UnityEngine;

public class BulletCollisionDetection : MonoBehaviour
{
    [Header("À set up")]

    [SerializeField] private LayerMask layerHasToStop;
    
    private Vector2 direction;

    private OnBulletHit onBulletHit;

    private Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        InvokeBulletHit();
    }

    private RaycastHit2D GetIntersection => Physics2D.CircleCast(transform.position, transform.localScale.x * 0.5f,
        transform.up,
        rb2D.velocity.magnitude * Time.fixedDeltaTime, layerHasToStop);

    private void InvokeBulletHit()
    {
        if (GetIntersection.collider == null)
        {
            return;
        }

        if (GetIntersection.collider.GetComponent<OnBulletHit>() != null)
        {
            onBulletHit = GetIntersection.collider.gameObject.GetComponent<OnBulletHit>();
            onBulletHit.BulletHitSomething();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DestroyObject"))
        {
            onBulletHit = other.gameObject.GetComponent<OnBulletHit>();
            onBulletHit.BulletHitSomething();
        }
    }

    public bool HasToStop()
    {
        if (!GetIntersection) return false;

        if ((layerHasToStop & 1 << GetIntersection.transform.gameObject.layer) ==
            1 << GetIntersection.transform.gameObject.layer)
            return true;

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + Vector3.right * 100 * Time.fixedDeltaTime,
            transform.localScale.x * 0.5f);
    }
}