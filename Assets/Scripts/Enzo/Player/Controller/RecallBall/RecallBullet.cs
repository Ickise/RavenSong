using UnityEngine;
using UnityEngine.InputSystem;

public class RecallBullet : MonoBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    private GameObject bullet;

    private Rigidbody2D bulletRigidbody;

    private Vector2 direction;
    private Vector2 distanceAmmoPlayer;

    public bool doRecall;

    private FireOneBullet _fireOneBullet;

    private BulletCollisionDetection _bulletCollisionDetection;

    [SerializeField] private float speedToRecall = 50f;
    [SerializeField] private float speedDelay = 0.2f;
    [SerializeField] private float distanceToRecall;

    private float distance;

    private float timer;

    private void Awake()
    {
        _fireOneBullet = GetComponentInChildren<FireOneBullet>();
    }

    private void Start()
    {
        InputReader.instance.onFire.AddListener(OnClickToRecall);
    }

    private void Update()
    {
        float delay = GetDelayBeforeMove();

        if (doRecall)
        {
            timer += Time.deltaTime;

            if (timer > delay)
            {
                RecallAmmo();
            }
        }

        if (_bulletCollisionDetection != null)
        {
            distanceAmmoPlayer = _bulletCollisionDetection.transform.position - transform.position;
        }

        if (InputReader.instance.jump || PlayerController2D._instance.onRoll ||
            InputReader.instance.canDown || InputReader.instance.canStun || !doRecall)
        {
            CancellRecall();
        }
    }

    private float GetDelayBeforeMove()
    {
        if (bullet != null)
        {
            direction =
                (PlayerController2D._instance.CurrentDirectionAim > 0 ? rightHand.position : leftHand.position) -
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
                CancellRecall();
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

    private void OnClickToRecall(InputAction.CallbackContext context)
    {
        GetBulletComponent();

        if (_bulletCollisionDetection == null) return;
        
        if (context.started && distanceAmmoPlayer.magnitude < distanceToRecall && _bulletCollisionDetection.hasToStop)
        {
            timer = 0;
            doRecall = true;
            //tous les feedbacks qui montrent qu'on att le recall, en faire une fonction
        }

        if (context.canceled)
        {
            CancellRecall();
        }
    }

    public void CancellRecall()
    {
        doRecall = false;
        //tous les feedbacks de l'annulation
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToRecall);
    }
}