using UnityEngine;

public class RecallBullet : MonoBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    private GameObject bullet;

    private Rigidbody2D bulletRigidbody;

    private Vector2 direction;
    private Vector2 distanceAmmoPlayer;

    private bool canRecall = false;
    public bool doRecall;

    private FireOneBullet _fireOneBullet;

    private BulletCollisionDetection _bulletCollisionDetection;

    [SerializeField] private float speedToRecall = 50f;
    [SerializeField] private float speedDelay = 0.2f;
    [SerializeField] private float distanceToRecall;

    private float distance;

    private void Awake()
    {
        _fireOneBullet = GetComponentInChildren<FireOneBullet>();
    }

    private void Start()
    {
        InputReader.instance.onRecall.AddListener(OnClickToRecall);
    }

    private void Update()
    {
        float delay = GetDelayBeforeMove();


        if (_bulletCollisionDetection != null)
        {
            distanceAmmoPlayer = _bulletCollisionDetection.transform.position - transform.position;
            
            if (_bulletCollisionDetection.hasToStop)
            {
                canRecall = true;
            }
        }

        if (canRecall && doRecall && distanceAmmoPlayer.magnitude < distanceToRecall)
        {
            Invoke("RecallAmmo", delay);
        }

        if (InputReader.instance.jump || PlayerController2D._instance.onRoll ||
            InputReader.instance.canDown || InputReader.instance.canStun || !doRecall)
        {
            //condition à modifier puisqu'elle est dégueu mais pour l'instant ça fera l'affaire
            doRecall = false;
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

    private void RecallAmmo()
    {
        if (bullet != null)
        {
            _bulletCollisionDetection.Recall();

            bulletRigidbody.velocity = direction.normalized * speedToRecall;

            if (_bulletCollisionDetection.touchPlayer.collider != null)
            {
                Destroy(bullet);
                _fireOneBullet.numberOfAmmo = 1;
                canRecall = false;
                doRecall = false;
            }
        }
    }

    private void GetBulletComponent()
    {
        bullet = _fireOneBullet.bulletRef;

        if (bullet == null)
        {
            return;
        }

        _bulletCollisionDetection = bullet.GetComponent<BulletCollisionDetection>();

        bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
    }

    private void OnClickToRecall(bool input)
    {
        GetBulletComponent();

        if (input && _bulletCollisionDetection.hasToStop)
        {
            doRecall = true;
        }
        else
        {
            doRecall = false;
        }
    }
}