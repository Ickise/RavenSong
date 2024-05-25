using System.Collections;
using Spine;
using UnityEngine;

public class StunDetection : MonoBehaviour
{
    private Collider2D c2D;
    public bool IsC2DActive => isCrossKickAnimationPlaying;
    [Header("À set up")]
    [SerializeField] private float crossKickCooldown = 2f;
    [SerializeField] private float timeToDisableHitBox = 0.3f;
    [SerializeField] private GameObject VFXAuraCoup, VFXCoupDeCrossObject;
    private PlayerAnimation _playerAnimation;
    private bool canCrossKick = true, isCrossKickAnimationPlaying;
    [Header("Sound")][SerializeField] private SoundData[] successfulStunSounds;
    [SerializeField] private SoundData unsuccessfulStunSound;
    private SoundData currentStunSound;


    private void Start()
    {
        c2D = GetComponent<Collider2D>();
        _playerAnimation = transform.parent.GetComponentInChildren<PlayerAnimation>();
    }

    public IEnumerator CrossKick(int currentDirection)
    {
        if (!canCrossKick) yield break;
        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.crossKickHaut, ReAim);
        // Debug.Log("CrossSong");
        //AudioManager.instance.PlaySound(soundstunnotsuccessful);
        _playerAnimation.DontAim = true;
        transform.localPosition = new Vector2(Mathf.Abs(transform.localPosition.x), transform.localPosition.y);
        transform.localPosition *= currentDirection;
        canCrossKick = false;
        c2D.enabled = true;
        currentStunSound = unsuccessfulStunSound;
        isCrossKickAnimationPlaying = true;
        yield return 3;
        AudioManager.instance.PlaySound(currentStunSound);
        c2D.enabled = false;
        yield return new WaitForSeconds(1);
        isCrossKickAnimationPlaying = false;
        yield return new WaitForSeconds(crossKickCooldown);
        canCrossKick = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //pour les IA
        IAProjectil iAprojectil = other.GetComponent<IAProjectil>();
        if (iAprojectil)
        {
            currentStunSound = successfulStunSounds[Random.Range(0, successfulStunSounds.Length)];
            VFXInstantieur.instance.PlayVFXInWorld(VFXCoupDeCrossObject, transform);
            iAprojectil.VFXStun.Play();
            VFXInstantieur.instance.PlayVFXInWorld(VFXAuraCoup, iAprojectil.transform);
            if (!iAprojectil.enabled)
                return;
            iAprojectil.ClearAnimations();
            iAprojectil.enabled = false;
            iAprojectil.StopAllCoroutines();
            iAprojectil.SetAnimation(IA.AnimationState.stunStart, iAprojectil.Stunning);
            return;
        }

        //pour les objets destructible
        Explodable destructibleObject = other.GetComponent<Explodable>();
        if (destructibleObject)
        {
            VFXInstantieur.instance.PlayVFXInWorld(VFXCoupDeCrossObject, transform);
            destructibleObject.explode(gameObject);
            ExplosionForce ef = FindObjectOfType<ExplosionForce>();
            ef.doExplosion(transform.position);
            return;
        }

        //tu peux rajouter d'autre condition ici

        OnBulletHit interactedObject = other.GetComponent<OnBulletHit>();
        IAHorloger iAHorloger = other.GetComponent<IAHorloger>();
        if (interactedObject && !iAHorloger)
        {
            //AudioManager.instance.PlayRandomSound(soundstunsuccessful);
            interactedObject.BulletHitSomething(null);
            return;
        }
    }

    private void ReAim(TrackEntry trackEntry)
    {
        _playerAnimation.DontAim = false;
    }
}