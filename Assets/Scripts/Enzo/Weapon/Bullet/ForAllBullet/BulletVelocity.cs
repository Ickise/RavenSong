using UnityEngine;

public class BulletVelocity : MonoBehaviour
{
    [Header("À set up")]

    [SerializeField] private float xSpeedBullet = 75f;

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
        baseBulletRigidbody2D.velocity = new Vector2(_bulletDirection.direction.x,_bulletDirection.direction.y).normalized * xSpeedBullet;
    }

    private void FixedUpdate()
    {
        Velocity();
    }

    private void Velocity()
    {
        //  AudioManager.instance.PlaySFX(impactAudio);
        
        if (_bulletCollisionDetection.HasToStop())
        {
            baseBulletRigidbody2D.velocity = Vector2.zero;
        }
    }
}