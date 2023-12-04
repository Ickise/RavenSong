using UnityEngine;

public class ThorBullet : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private Rigidbody2D thorBulletRigidbody2D;

    [SerializeField] private float xBulletSpeed = 25f;

    [SerializeField] private BulletDirection _bulletDirection;
    [SerializeField] private SphereCastDetection _sphereCastDetection;
    [SerializeField] private PlayerController2D _player;

    [SerializeField] private AudioClip impactAudio;

    private float t;
    
    private void OnEnable()
    {
        _bulletDirection.Init();
        _player = FindObjectOfType<PlayerController2D>();
    }

    private void Update()
    {
        t += 0.5f * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Velocity();
    }

    private void Velocity()
    {
        if (_sphereCastDetection.isTouchingWall)
        {
            AudioManager.instance.PlaySFX(impactAudio);
            
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, _player.transform.position.x,t), Mathf.Lerp(transform.position.y, _player.transform.position.y,t));
            thorBulletRigidbody2D.velocity = -_bulletDirection.direction.normalized * xBulletSpeed;
        }
        else
        {
            thorBulletRigidbody2D.velocity = _bulletDirection.direction.normalized * xBulletSpeed;
        }
    }
}