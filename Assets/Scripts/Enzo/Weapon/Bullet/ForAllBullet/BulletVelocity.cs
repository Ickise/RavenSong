using UnityEngine;

public class BulletVelocity : MonoBehaviour
{
    [Header("À set up")] [SerializeField] private Rigidbody2D baseBulletRigidbody2D;

    [SerializeField] private float xSpeedBullet = 15f;

    [SerializeField] private BulletDirection _bulletDirection;
    [SerializeField] private BulletCollisionDetection _bulletCollisionDetection;

    //[SerializeField] private AudioClip impactAudio;

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