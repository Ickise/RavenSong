using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class RecallBullet : MonoBehaviour
{
    [SerializeField] private Transform rightHand, leftHand;
    [SerializeField] private GameObject vfxRecallBulletDroite, vfxRecallBulletGauche, vfxTrailRecall;

    private Rigidbody2D bulletRigidbody;

    private Vector2 direction;
    private Vector2 distanceAmmoPlayer;

    public bool doRecall;
    public bool onRecall;

    private PlayerAnimation _playerAnimation;

    private BulletCollisionDetection _bulletCollisionDetection;

    [SerializeField] private float speedToRecall = 50f;
    [SerializeField] private float speedDelay = 0.2f;
    [SerializeField] private float distanceToRecall;
    [SerializeField] private float distanceToGetAmmo = 0.5f;

    private float timer;
    private float delay;

    private void Awake()
    {
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
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
            vfxTrailRecall.transform.position = vfxRecallBulletDroite.activeInHierarchy ? vfxRecallBulletDroite.transform.position : vfxRecallBulletGauche.transform.position;
            vfxTrailRecall.GetComponent<VisualEffect>().SetVector3("Ball_Position", _bulletCollisionDetection.transform.position - vfxTrailRecall.transform.position);
            timer += Time.deltaTime;

            if (timer > delay)
            {
                RecallAmmo();
            }
        }
    }

    private float GetDelayBeforeMove()
    {
        if (FireOneBullet.instance.bulletRef != null)
        {
            direction =
                (PlayerController2D._instance.CurrentDirectionAim > 0 ? rightHand.position : leftHand.position) -
                FireOneBullet.instance.bulletRef.transform.position;
            float distance = direction.magnitude;

            return distance * speedDelay;
        }

        return 0;
    }

    private void RecallAmmo()
    {
        if (FireOneBullet.instance.bulletRef != null)
        {
            _bulletCollisionDetection.Recall();

            onRecall = true;

            GetBulletToReload();
        }
    }

    private void GetBulletToReload()
    {
        if (distanceAmmoPlayer.magnitude < distanceToGetAmmo)
        {
            Destroy(FireOneBullet.instance.bulletRef);
            FireOneBullet.instance.numberOfAmmo = 1;
            onRecall = false;
            CancellRecall();
        }
    }

    private void GetBulletComponent()
    {
        if (FireOneBullet.instance.bulletRef == null) return;

        _bulletCollisionDetection = FireOneBullet.instance.bulletRef.GetComponent<BulletCollisionDetection>();

        bulletRigidbody = FireOneBullet.instance.bulletRef.GetComponent<Rigidbody2D>();
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
                vfxTrailRecall.SetActive(true);
            if (_playerAnimation.GetDirection)
                vfxRecallBulletDroite.SetActive(true);
            else
                vfxRecallBulletGauche.SetActive(true);
            _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.recallHaut);
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
        vfxRecallBulletDroite.SetActive(false);
        vfxRecallBulletGauche.SetActive(false);
        vfxTrailRecall.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToRecall);
    }
}