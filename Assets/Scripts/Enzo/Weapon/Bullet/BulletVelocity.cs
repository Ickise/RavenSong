using UnityEngine;

public class BulletVelocity : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private Rigidbody2D baseBulletRigidbody2D;

    [SerializeField] private float xSpeedBullet = 25f;
    [SerializeField] private float xBounciness = 0.3f;
    [SerializeField] private float yBounciness = -3.3f;

    [SerializeField] private BulletDirection _bulletDirection;
    [SerializeField] private SphereCastDetection _sphereCastDetection;
    
    [SerializeField] private AudioClip impactAudio;
    
    private bool hasBounced = false;
    
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
        if (_sphereCastDetection.hitWall && !hasBounced)
        {
            AudioManager.instance.PlaySFX(impactAudio);
            
            baseBulletRigidbody2D.velocity = new Vector2(-baseBulletRigidbody2D.velocity.x * xBounciness, yBounciness);
            hasBounced = true;
        }
        else if (!hasBounced)
        { 
            baseBulletRigidbody2D.velocity = _bulletDirection.direction.normalized * xSpeedBullet;
        }

        if (_sphereCastDetection.hitGround)
        {
            baseBulletRigidbody2D.velocity = Vector2.zero;
        }
    }
}