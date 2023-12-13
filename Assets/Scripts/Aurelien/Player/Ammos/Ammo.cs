using UnityEngine;
using System.Collections;

public abstract class Ammo : MonoBehaviour
{
    [SerializeField] protected bool canPlayerRecover = true;
    [SerializeField] protected LayerMask layerBall;
    public bool CanRecover { get; set; }
    public bool IsRecover { get; protected set; }
    protected Rigidbody2D rb2D;
    protected Collider2D c2D;
    [SerializeField] protected float vitesseRecuperation = 200f, force = 200f, forceRecule = 15f, vitesseMinimumEffect = 75f;
    public Gun _gun { get; set; }
    protected float ammoMagnitude;
    protected bool active = true;

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        c2D = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        IsRecover = false;
        CanRecover = false;
        rb2D.AddForce(transform.up * force, ForceMode2D.Impulse);
    }

    protected virtual void Update()
    {
        if (IsRecover)
        {
            transform.position = Vector3.MoveTowards(transform.position, _gun.transform.position, vitesseRecuperation * Time.deltaTime);
            if (Vector2.Distance(_gun.transform.position, transform.position) < 1.3f)
                Destroy(gameObject);
        }
        if (Vector2.Distance(_gun.transform.position, transform.position) < 1.3f)
            Destroy(gameObject);
        // if (CanRecover || canPlayerRecover)
        //     if (Vector2.Distance(_gun.transform.position, transform.position) < 1.3f)
        //         Destroy(gameObject);
        ammoMagnitude = rb2D.velocity.magnitude;
        if (ammoMagnitude < vitesseMinimumEffect)
        {
            gameObject.layer = LayerMask.NameToLayer("IADontCollide");
            active = false;
        }
    }

    protected virtual void FixedUpdate()
    {
        // Debug.DrawRay(transform.position, transform.up * 1.2f * rb2D.velocity.magnitude * Time.fixedDeltaTime);
        if (!active) return;
        RaycastHit2D hit2D = Physics2D.CircleCast(transform.position, transform.localScale.x, transform.up, rb2D.velocity.magnitude * Time.fixedDeltaTime, layerBall);
        if (hit2D)
        {
            StartCoroutine(Wait1frame(hit2D));
            IEnumerator Wait1frame(RaycastHit2D hit2D)
            {
                yield return 0;
                active = false;
                Trigger(hit2D);
                if (hit2D.transform.CompareTag("DestroyObject"))
                {
                    Explodable explodableObj = hit2D.transform.GetComponent<Explodable>();
                    explodableObj.explode();
                    ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
                    ef.doExplosion(transform.position);
                }
            }
        }
    }

    protected abstract void Trigger(RaycastHit2D hit2D);
    public abstract void Recover();
}
