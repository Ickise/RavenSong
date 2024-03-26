using UnityEngine;

public class BulletVelocity : MonoBehaviour
{
    [SerializeField,Header("Speed bullet"), Range(1f, 100f)] private float xSpeedBullet = 50f;
    [SerializeField, Header("Fall speed bullet"), Range(1f, 20f)] private float forceToFall = 0.5f;
    
    //[SerializeField] private AudioClip impactAudio;

    private Rigidbody2D bulletRigidbody2D;

    private BulletDirection _bulletDirection;
    private BulletCollisionDetection _bulletCollisionDetection;

    private void Awake()
    {
        _bulletDirection = GetComponent<BulletDirection>();
        _bulletCollisionDetection = GetComponent<BulletCollisionDetection>();
        bulletRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        bulletRigidbody2D.velocity =
            new Vector2(_bulletDirection.direction.x, _bulletDirection.direction.y).normalized * xSpeedBullet;
    }

    public void SetVelocityOnHit()
    {
        //  AudioManager.instance.PlaySFX(impactAudio);

        if (_bulletCollisionDetection.hasToStop)
        {
            bulletRigidbody2D.velocity = Vector2.down * forceToFall;
        }

        if (_bulletCollisionDetection.onBulletHit != null && !_bulletCollisionDetection.onBulletHit.bulletFalling)
        {
            bulletRigidbody2D.velocity = Vector2.zero;
        }
    }
}