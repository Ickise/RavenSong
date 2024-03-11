using System.Collections;
using Spine;
using UnityEngine;
using UnityEngine.VFX;

public class StunDetection : MonoBehaviour
{
    private Collider2D c2D;
    public bool IsC2DActive => c2D.enabled;
    [Header("À set up")][SerializeField] private float stunDuration = 1.5f;
    [SerializeField] private float crossKickCooldown = 2f;
    [SerializeField] private float timeToDisableHitBox = 0.3f;
    [SerializeField] private GameObject VFXAuraCoup;
    [SerializeField] private VisualEffect VFXCoupDeCross;
    private PlayerAnimation _playerAnimation;
    private bool canCrossKick = true;

    private void Start()
    {
        c2D = GetComponent<Collider2D>();
        _playerAnimation = transform.parent.GetComponentInChildren<PlayerAnimation>();
    }

    public void CrossKick(int currentDirection)
    {
        if (!canCrossKick) return;
        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.crossKickHaut);
        transform.localPosition = new Vector2(Mathf.Abs(transform.localPosition.x), transform.localPosition.y);
        transform.localPosition *= currentDirection;
        canCrossKick = false;
        StartCoroutine(CoolDown());
        IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(crossKickCooldown);
            canCrossKick = true;
        }
        c2D.enabled = true;
        StartCoroutine(TimeHitBox());
        IEnumerator TimeHitBox()
        {
            yield return new WaitForSeconds(stunDuration);
            c2D.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //pour les IA
        IA iA = other.GetComponent<IA>();
        if (iA)
        {
            VFXCoupDeCross.Play();
            iA.VFXStun.Play();
            GameObject currentVFXAuraCoup = Instantiate(VFXAuraCoup, iA.transform);
            Destroy(currentVFXAuraCoup, 3);
            iA.enabled = false;
            StartCoroutine(TimeUnstun());
            IEnumerator TimeUnstun()
            {
                iA.SetAnimation(IA.AnimationState.shoot);
                yield return new WaitForSeconds(stunDuration);
                iA.enabled = true;
                iA.VFXStun.Stop();
            }
            return;
        }

        //pour les objets destructible
        Explodable destructibleObject = other.GetComponent<Explodable>();
        if (destructibleObject)
        {
            destructibleObject.explode(gameObject);
            ExplosionForce ef = FindObjectOfType<ExplosionForce>();
            ef.doExplosion(transform.position);
            return;
        }

        //tu peux rajouter d'autre condition ici

        OnBulletHit interactedObject = other.GetComponent<OnBulletHit>();
        if (interactedObject)
        {
            interactedObject.BulletHitSomething(null);
        }
    }
    public void Function(TrackEntry trackEntry)
    {
        // trackEntry.
    }
}