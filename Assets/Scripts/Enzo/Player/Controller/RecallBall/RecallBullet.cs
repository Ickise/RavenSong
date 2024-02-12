using UnityEngine;

public class RecallBullet : MonoBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    private GameObject bullet;

    private Rigidbody2D bulletRigidbody;

    private Vector2 direction;

    private bool isMoving;

    private FireOneBullet _fireOneBullet;

    private BulletCollisionDetection _bulletCollisionDetection;

    [SerializeField] private float destroyDistance = 0.6f;
    [SerializeField] private float speedToRecall = 50f;
    [SerializeField] private float speedDelay = 0.2f;
    [SerializeField] private float distanceToRecall = 10f;

    private float distance;

    private void Awake()
    {
        _fireOneBullet = GetComponentInChildren<FireOneBullet>();
    }

    private void Start()
    {
        InputReader.instance.onRecall.AddListener(OnClick);
    }

    private void Update()
    {
        float delay = GetDelayBeforeMove();

        if (InputReader.instance.canRecall && transform.position.x < distanceToRecall)
        {
            Invoke("RecallAmmo", delay);
        }
        else
        {
            CancelInvoke("RecallAmmo");
        }
    }

    private float GetDelayBeforeMove()
    {
        if (bullet != null)
        {
            direction = (PlayerController2D._instance.CurrentDirection > 0 ? rightHand.position : leftHand.position) -
                        bullet.transform.position;
            distance = direction.magnitude;

            return distance * speedDelay;
        }

        return 0;
    }

    public void RecallAmmo()
    {
        if (bullet != null)
        {
            _bulletCollisionDetection.Recall();

            bulletRigidbody.velocity = direction.normalized * speedToRecall;

            if (distance <= destroyDistance)
            {
                Destroy(bullet);
                _fireOneBullet.numberOfAmmo = 1;
            }
        }
    }

    private void OnClick()
    {
        bullet = _fireOneBullet.bulletRef;

        if (bullet == null)
        {
            return;
        }

        _bulletCollisionDetection = bullet.GetComponent<BulletCollisionDetection>();

        bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
    }
}