using UnityEngine;
using UnityEngine.VFX;

public class BulletCollisionDetection : MonoBehaviour
{
    [SerializeField, Header("LayerCollision")]
    private LayerMask bulletCollision;

    [SerializeField, Header("VFX")] private VisualEffect VFXRaisonnanceBall;
    [SerializeField] private VisualEffect VFXExplosionImpact;

    [SerializeField, Header("RadiusToDetectCollision"), Range(0, 1)]
    private float radius = 0.08f;

    private BulletVelocity _bulletVelocity;

    private Rigidbody2D bulletRigidbody2D;
    private Collider2D bulletCollider;

    private RaycastHit2D intersection;

    [HideInInspector] public OnBulletHit onBulletHit;

    [HideInInspector] public bool hasToStop;

    private void Awake()
    {
        GetAndSetComponent();
    }

    private void FixedUpdate()
    {
        if (hasToStop) return;

        RaycastHit2D hit2D = Physics2D.CircleCast(
            transform.position + new Vector3(bulletRigidbody2D.velocity.normalized.x,
                bulletRigidbody2D.velocity.normalized.y, 0) *
            Time.fixedDeltaTime, radius, bulletRigidbody2D.velocity * Time.fixedDeltaTime,
            bulletRigidbody2D.velocity.magnitude * Time.fixedDeltaTime, bulletCollision);
        if (!hit2D) return;
        if (hit2D.transform.CompareTag("Platforme")) return;

        if (hit2D.collider == intersection.collider) return;

        intersection = hit2D;

        InvokeBulletHit();
    }

    private void InvokeBulletHit()
    {
        onBulletHit = intersection.collider.GetComponentInParent<OnBulletHit>();

        if (onBulletHit == null)
        {
            if (intersection.collider.TryGetComponent(out onBulletHit) == false)
            {
                hasToStop = true;
                FixBulletOnObject();
                return;
            }
        }

        hasToStop = !onBulletHit.canGoThrough;

        if (hasToStop)
        {
            FixBulletOnObject();
        }

        IAHorloger _iAChargeur = onBulletHit.GetComponent<IA>() as IAHorloger;
        if (_iAChargeur && _iAChargeur.IsAttacking)
            if (_iAChargeur.GetDirection
                    ? transform.position.x > _iAChargeur.transform.position.x
                    : transform.position.x < _iAChargeur.transform.position.x)
                return;
        onBulletHit.BulletHitSomething(gameObject);
    }

    private void FixBulletOnObject()
    {
        transform.position = intersection.point;
        VFXRaisonnanceBall.Play();
        VFXExplosionImpact.Play();

        ChangeColliderRigidbody(true);

        _bulletVelocity.SetVelocityOnHit();
    }

    public void Recall()
    {
        transform.parent = null;
        VFXRaisonnanceBall.Stop();
        ChangeColliderRigidbody(false);
        hasToStop = false;
        //hasToStop en false sur le rappel permet de toucher les objets sur le retour.
    }

    private void ChangeColliderRigidbody(bool isEnable)
    {
        bulletCollider.enabled = isEnable;
        bulletRigidbody2D.isKinematic = !bulletCollider.enabled;
    }

    private void GetAndSetComponent()
    {
        bulletRigidbody2D = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();
        _bulletVelocity = GetComponent<BulletVelocity>();
        bulletCollider.enabled = false;
        VFXRaisonnanceBall.Stop();
        VFXExplosionImpact.Stop();
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