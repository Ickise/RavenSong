using UnityEngine;
using UnityEngine.VFX;

public class BulletCollisionDetection : MonoBehaviour
{
    [Header("À set up")] [SerializeField] private LayerMask bulletCollision;
    [SerializeField] private LayerMask layerPlayer;

    [SerializeField] private float radius = 0.25f;

    private OnBulletHit onBulletHit;

    private Rigidbody2D rb2D;

    RaycastHit2D intersection;
    public RaycastHit2D touchPlayer;

    [SerializeField] private VisualEffect VFXRaisonnanceBall, VFXExplosionImpact;

    public bool hasToStop;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        VFXRaisonnanceBall.Stop();
        VFXExplosionImpact.Stop();
    }

    private void FixedUpdate()
    {
        if (hasToStop)
        {
            return;
        }

        RaycastHit2D hit2D = Physics2D.CircleCast(
            transform.position + new Vector3(rb2D.velocity.normalized.x, rb2D.velocity.normalized.y, 0) *
            Time.fixedDeltaTime, radius, rb2D.velocity * Time.fixedDeltaTime,
            rb2D.velocity.magnitude * Time.fixedDeltaTime, bulletCollision);

        touchPlayer = Physics2D.CircleCast(
            transform.position + new Vector3(rb2D.velocity.normalized.x, rb2D.velocity.normalized.y, 0) *
            Time.fixedDeltaTime, radius, rb2D.velocity * Time.fixedDeltaTime,
            rb2D.velocity.magnitude * Time.fixedDeltaTime, layerPlayer);

        if (hit2D.collider == intersection.collider)
        {
            return;
        }

        intersection = hit2D;

        InvokeBulletHit();
    }

    private void InvokeBulletHit()
    {
        if (intersection.collider == null)
        {
            return;
        }

        if (intersection.collider.TryGetComponent(out onBulletHit) == false)
        {
            hasToStop = true;
            FixBulletOnObject();
            return;
        }

        hasToStop = !onBulletHit.canGoThrough;

        if (hasToStop)
        {
            FixBulletOnObject();
        }

        onBulletHit.BulletHitSomething(gameObject);
    }

    private void FixBulletOnObject()
    {
        transform.position = intersection.point;
        transform.parent = intersection.collider.transform;
        VFXRaisonnanceBall.Play();
        VFXExplosionImpact.Play();
    }


    public void Recall()
    {
        transform.parent = null;
        VFXRaisonnanceBall.Stop();
        hasToStop = false;
        //hasToStop en false sur le rappel permet de toucher les objets sur le retour.
    }

    /*private void OnDrawGizmos()
    {
        if (intersection.collider)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(intersection.point, radius);
        }
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, radius);
        
        Gizmos.DrawSphere(transform.position + new Vector3(rb2D.velocity.normalized.x, rb2D.velocity.normalized.y, 0) * radius, radius);
        
        Debug.DrawRay(transform.position +  new Vector3(rb2D.velocity.normalized.x, rb2D.velocity.normalized.y, 0) * Time.fixedDeltaTime, rb2D.velocity);
    }*/
}