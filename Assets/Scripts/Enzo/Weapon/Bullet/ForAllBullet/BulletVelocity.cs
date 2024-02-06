using UnityEngine;

public class BulletVelocity : MonoBehaviour
{
    [Header("À set up")]

    [SerializeField] private float xSpeedBullet = 15f;

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

    private void OnEnable()
    {
        _bulletDirection.Init();
    }

    private void FixedUpdate()
    {
        Velocity();
    }

    private void Velocity()
    {
        //  AudioManager.instance.PlaySFX(impactAudio);

        baseBulletRigidbody2D.velocity = _bulletDirection.direction.normalized * xSpeedBullet;

        if (_bulletCollisionDetection.hitGround || _bulletCollisionDetection.hitSomething ||
            InputReader.instance.canRecall)
        {
            baseBulletRigidbody2D.velocity = Vector2.zero;
        }
    }
}