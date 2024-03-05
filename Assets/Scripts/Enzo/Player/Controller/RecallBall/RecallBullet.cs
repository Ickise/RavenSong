using UnityEngine;
using UnityEngine.InputSystem;

public class RecallBullet : MonoBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    private Rigidbody2D bulletRigidbody;

    private Vector2 direction;
    private Vector2 distanceAmmoPlayer;

    public bool doRecall;
    public bool onRecall;

    private FireOneBullet _fireOneBullet;

    private BulletCollisionDetection _bulletCollisionDetection;

    [SerializeField] private float speedToRecall = 50f;
    [SerializeField] private float speedDelay = 0.2f;
    [SerializeField] private float distanceToRecall;

    private float timer;
    private float delay;

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
        delay = GetDelayBeforeMove();

        LaunchRecall();

        if (_bulletCollisionDetection != null)
        {
            distanceAmmoPlayer = _bulletCollisionDetection.transform.position - transform.position;
        }

        if (onRecall && bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction.normalized * speedToRecall;

            GetBulletToReload();
        }

        if (InputReader.instance.jump || PlayerController2D._instance.onRoll ||
            InputReader.instance.canDown || InputReader.instance.canStun || !doRecall)
        {
            CancellRecall();
        }
    }

    private void LaunchRecall()
    {
        if (doRecall)
        {
            timer += Time.deltaTime;

            if (timer > delay)
            {
                RecallAmmo();
            }
        }
    }

    private float GetDelayBeforeMove()
    {
        if (_fireOneBullet.bulletRef != null)
        {
            direction =
                (PlayerController2D._instance.CurrentDirectionAim > 0 ? rightHand.position : leftHand.position) -
                _fireOneBullet.bulletRef.transform.position;
            float distance = direction.magnitude;

            return distance * speedDelay;
        }

        return 0;
    }

    private void RecallAmmo()
    {
        if (_fireOneBullet.bulletRef != null)
        {
            _bulletCollisionDetection.Recall();

            onRecall = true;

            GetBulletToReload();
        }
    }

    private void GetBulletToReload()
    {
        if (_bulletCollisionDetection.touchPlayer.collider != null)
        {
            Destroy(_fireOneBullet.bulletRef);
            _fireOneBullet.numberOfAmmo = 1;
            onRecall = false;
            CancellRecall();
        }
    }

    private void GetBulletComponent()
    {
        if (_fireOneBullet.bulletRef == null) return;

        _bulletCollisionDetection = _fireOneBullet.bulletRef.GetComponent<BulletCollisionDetection>();

        bulletRigidbody = _fireOneBullet.bulletRef.GetComponent<Rigidbody2D>();
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