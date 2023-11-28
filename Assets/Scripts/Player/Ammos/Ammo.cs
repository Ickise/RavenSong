using UnityEngine;

public abstract class Ammo : MonoBehaviour
{
    [SerializeField] protected bool canPlayerRecover = true;
    public bool CanRecover { get; set; }
    public bool IsRecover { get; protected set; }
    protected Rigidbody2D rb2D;
    protected Collider2D c2D;
    [SerializeField] protected float speedRecover, force, forceRecule;
    protected Gun _gun;
    protected float ammoMagnitude;

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
            transform.position = Vector3.MoveTowards(transform.position, _gun.transform.position, speedRecover * Time.deltaTime);
            if (Vector2.Distance(_gun.transform.position, transform.position) < 1.3f)
                Destroy(gameObject);
        }
        // if (CanRecover || canPlayerRecover)
        //     if (Vector2.Distance(_gun.transform.position, transform.position) < 1.3f)
        //         Destroy(gameObject);
        ammoMagnitude = rb2D.velocity.magnitude;
    }

    protected abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void Recover(Gun _gun);
}
