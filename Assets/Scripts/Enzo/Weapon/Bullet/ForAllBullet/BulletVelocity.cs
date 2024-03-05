using UnityEngine;

public class BulletVelocity : MonoBehaviour
{
    [Header("À set up")] [SerializeField] private float xSpeedBullet = 75f;
    [SerializeField] private float forceToFall = 0.5f;

    private Rigidbody2D baseBulletRigidbody2D;

    private BulletDirection _bulletDirection;
    private BulletCollisionDetection _bulletCollisionDetection;

    //[SerializeField] private AudioClip impactAudio;

    private void Awake()
    {
        _bulletDirection = GetComponent<BulletDirection>();
        _bulletCollisionDetection = GetComponent<BulletCollisionDetection>();
        baseBulletRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        baseBulletRigidbody2D.velocity =
            new Vector2(_bulletDirection.direction.x, _bulletDirection.direction.y).normalized * xSpeedBullet;
    }

    public void SetVelocityOnHit()
    {
        //  AudioManager.instance.PlaySFX(impactAudio);

        if (_bulletCollisionDetection.hasToStop)
        {
            baseBulletRigidbody2D.velocity = Vector2.down * forceToFall;
        }

        if (_bulletCollisionDetection.onBulletHit != null && !_bulletCollisionDetection.onBulletHit.bulletFalling)
        {
            baseBulletRigidbody2D.velocity = Vector2.zero;
        }
    }
}