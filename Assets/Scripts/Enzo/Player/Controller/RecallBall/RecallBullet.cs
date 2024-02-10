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
            bulletRigidbody.velocity = Vector2.zero;

            bullet.transform.Translate(direction.normalized * (speedToRecall * Time.deltaTime));

            if (distance <= destroyDistance)
            {
                Destroy(bullet);
                _fireOneBullet.numberOfAmmo = 1;
            }
        }
    }

    private void OnClick()
    {
        bullet = FindObjectOfType<BulletCollisionDetection>().gameObject;

        if (bullet != null)
        {
            bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            Destroy(bullet.GetComponent<BulletVelocity>());
        }
    }
}