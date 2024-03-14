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

    public IEnumerator CrossKick(int currentDirection)
    {
        if (!canCrossKick) yield break;
        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.crossKickHaut);
        _playerAnimation.DontAim = true;
        transform.localPosition = new Vector2(Mathf.Abs(transform.localPosition.x), transform.localPosition.y);
        transform.localPosition *= currentDirection;
        canCrossKick = false;
        c2D.enabled = true;
        yield return new WaitForSeconds(stunDuration);
        c2D.enabled = false;
        StartCoroutine(CrossKickCoolDown());
        IEnumerator CrossKickCoolDown()
        {
            yield return new WaitForSeconds(crossKickCooldown);
            _playerAnimation.DontAim = false;
            canCrossKick = true;
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
            iA.ClearAnimations();
            iA.enabled = false;
            iA.SetAnimation(IA.AnimationState.stunStart, iA.Stunning);
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
}