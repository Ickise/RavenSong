using UnityEngine;

public class BulletVelocity : MonoBehaviour
{
    [SerializeField, Header("Speed bullet"), Range(1f, 100f)]
    private float xSpeedBullet = 50f;

    [SerializeField, Header("Fall speed bullet"), Range(1f, 20f)]
    private float forceToFall = 0.5f;

    [SerializeField, Header("Maximum distance before to fall")]
    private float maxDistanceToFall = 50f;

    private Rigidbody2D bulletRigidbody2D;

    private BulletDirection _bulletDirection;
    private BulletCollisionDetection _bulletCollisionDetection;

    private float totalDistance = 0;

    private void Awake()
    {
        _bulletDirection = GetComponent<BulletDirection>();
        _bulletCollisionDetection = GetComponent<BulletCollisionDetection>();
        bulletRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        bulletRigidbody2D.velocity =
            new Vector2(_bulletDirection.direction.x, _bulletDirection.direction.y).normalized *
            xSpeedBullet;
    }

    private void FixedUpdate()
    {
        if (_bulletCollisionDetection.hasToStop) return; // si jamais problème de recall, voir ici
        RecordDistance();
        SetVelocityOnMaxDistance();
    }

    public void SetVelocityOnHit()
    {
        if (_bulletCollisionDetection.hasToStop)
        {
            //mettre son quand elle commence à tomber
            bulletRigidbody2D.velocity = Vector2.down * forceToFall;
        }

        if (_bulletCollisionDetection.onBulletHit != null && !_bulletCollisionDetection.onBulletHit.bulletFalling)
        {
            bulletRigidbody2D.velocity = Vector2.zero;
        }
    }

    private void SetVelocityOnMaxDistance()
    {
        bool achieveMaxDistance = totalDistance >= maxDistanceToFall;

        if (achieveMaxDistance)
        {
            //mettre son quand elle commence à tomber
            bulletRigidbody2D.velocity = Vector2.down * forceToFall;
        }
    }

    private void RecordDistance()
    {
        totalDistance = Vector3.Distance(transform.position, PlayerController2D._instance.transform.position);
    }
}