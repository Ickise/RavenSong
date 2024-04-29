using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class RecallBullet : MonoBehaviour
{
    [SerializeField, Header("Transform des mains")]
    private Transform rightHand;

    [SerializeField] private Transform leftHand;
    [SerializeField, Header("VFX")] private GameObject vfxRecallBulletDroite;
    [SerializeField] private GameObject vfxRecallBulletGauche;
    [SerializeField] private GameObject vfxTrailRecall;
    private VisualEffect visualEffectTrailRecall;

    private Rigidbody2D bulletRigidbody;

    private Vector2 direction;
    private Vector2 distanceAmmoPlayer;

    [HideInInspector] public bool doRecall;
    [HideInInspector] public bool onRecall;

    private PlayerAnimation _playerAnimation;

    private BulletCollisionDetection _bulletCollisionDetection;

    [SerializeField, Header("Vitesse de rappel")]
    private float speedBulletRecall = 50f;

    [SerializeField] private float speedDelay = 0.2f;

    [SerializeField,
     Tooltip(
         "Temps de rappel de la balle lorsqu'elle est dans un cadavre. Peu importe la distance entre le cadavre et le joueur, elle mettra toujours le même temps de rappel.")]
    private float delayBulletRecallInCorpse = 1f;

    [SerializeField, Header("Distance pour récupérer la balle")]
    private float distanceToGetAmmo = 0.5f;

    [SerializeField, Header("Distance de rappel")]
    private float distanceToRecall;

    private float timer;
    private float delay;

    private void Awake()
    {
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        visualEffectTrailRecall = vfxTrailRecall.GetComponent<VisualEffect>();
    }

    private void Start()
    {
        InputReader.instance.onFire.AddListener(OnClickToRecall);
    }

    private void Update()
    {
        delay = DestroyEnemyCorpse.bulletInCorpse ? delayBulletRecallInCorpse : GetDelayBeforeMove();

        LaunchRecall();

        if (_bulletCollisionDetection != null)
        {
            distanceAmmoPlayer = _bulletCollisionDetection.transform.position - transform.position;
        }

        if (onRecall && bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction.normalized * speedBulletRecall;

            GetBulletToReload();
        }

        if (InputReader.instance.jump || PlayerController2D._instance.onRoll ||
            InputReader.instance.canDown || InputReader.instance.canStun || !doRecall)
        {
            CancelRecall();
        }
    }

    private void LaunchRecall()
    {
        if (doRecall)
        {
            vfxTrailRecall.transform.position = vfxRecallBulletDroite.activeInHierarchy
                ? vfxRecallBulletDroite.transform.position
                : vfxRecallBulletGauche.transform.position;
            visualEffectTrailRecall.SetVector3("Ball_Position",
                _bulletCollisionDetection.transform.position - vfxTrailRecall.transform.position);
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
            DestroyEnemyCorpse.bulletInCorpse = false;
            CancelRecall();
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
            _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.recallHaut, -1,  8f /Vector2.Distance(bulletRigidbody.transform.position, transform.position));
        }

        if (context.canceled)
        {
            CancelRecall();
        }
    }

    private void CancelRecall()
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