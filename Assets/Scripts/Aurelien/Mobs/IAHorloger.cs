using System.Collections;
using UnityEngine;
using DG.Tweening;
using Spine;

public class IAHorloger : IA
{
    [SerializeField] private float reloadTime = 5f, runTime = 5f, decelerationTime = 1f;
    [SerializeField, Tooltip("le temps entre le moment ou il voit le joueur et il commence à le charger")] private float beforeChargeTime = 1f;
    [SerializeField] private Collider2D cd2Datk;
    public bool IsAttacking => cd2Datk.enabled;
    [SerializeField] private GameObject VFXBonk, VFXCourse, VFXCourseEtincel;
    [SerializeField] private Transform posVFXBonk, posVFXCourse, posVFXCourseEtincel;
    private OnBulletHit _onBulletHit;
    private bool isReloading;
    private State state;
    private enum State
    {
        WaitPlayer,
        ChasePlayer
    }

    protected override void Start()
    {
        base.Start();
        _onBulletHit = GetComponent<OnBulletHit>();
        tailleMob.y += 0.2f;
        overwriteIniTialize = true;
    }

    protected override void StateManager()
    {
        switch (state)
        {
            default:
            case State.WaitPlayer:
                IsWaitingPlayer();
                break;
            case State.ChasePlayer:
                ChasePlayer();
                IsChasePlayer();
                break;
        }
    }

    private void IsWaitingPlayer()
    {
        if (isReloading || DOTween.IsTweening("chargingTime")) return;
        if (DetectPlayer)
        {
            direction = transform.position.x < player.position.x;
            float a = 0;
            DOTween.To(() => a, x => a = x, 1f, beforeChargeTime).SetId("chargingTime")
            // ;spriteRenderer.DOColor(Color.red, chargeTime / 4f)
            .OnComplete(() =>
            {
                VFXInstantieur.instance.PlayVFXInWorld(VFXCourse, posVFXCourse);
                VFXInstantieur.instance.PlayVFXInWorld(VFXCourseEtincel, posVFXCourse);
                state = State.ChasePlayer;
                _onBulletHit.bulletFalling = true;
                SetAnimation(AnimationState.moveForward);
                StartCoroutine(RunTime());
            });
            IEnumerator RunTime()
            {
                yield return new WaitForSeconds(runTime);
                DOTween.To(() => rb2D.velocity, x => rb2D.velocity = x, Vector2.zero, decelerationTime);
                isReloading = true;
                StartCoroutine(StartReload());
                cd2Datk.enabled = false;
                state = State.WaitPlayer;
                _onBulletHit.bulletFalling = false;
            }
        }
    }

    private void ChasePlayer()
    {
        cd2Datk.enabled = true;
        rb2D.velocity = new Vector2(direction ? speedAttaquePlayer : -speedAttaquePlayer, rb2D.velocity.y);
        transform.localScale = direction ? Vector2.one : new Vector2(-1, 1);
    }

    private void IsChasePlayer()
    {

        if (RaycastHitWall)
        {
            if (RaycastHitWall.transform.CompareTag("Platforme"))
            {
                if (!Physics2D.GetIgnoreCollision(RaycastHitWall.collider, cd2D))
                {
                    Physics2D.IgnoreCollision(RaycastHitWall.collider, cd2D);
                    // Physics2D.IgnoreCollision(RaycastHitWall.transform.GetChild(0).GetComponent<Collider2D>(), cd2D);
                }
            }
            else if (!RaycastHitWall.collider.GetComponent<StunDetection>())
            {
                OnBulletHit interactedObject = RaycastHitWall.collider.GetComponent<OnBulletHit>();
                if (interactedObject)
                    interactedObject.BulletHitSomething(null);

                cd2Datk.enabled = false;
                rb2D.velocity = Vector2.zero;
                rb2D.AddForce((direction ? new Vector2(-1, 1) : Vector2.one) * 5f, ForceMode2D.Impulse);
                SetAnimation(AnimationState.bonk);
                VFXInstantieur.instance.PlayVFXInWorld(VFXBonk, posVFXBonk, 3);
                state = State.WaitPlayer;
                _onBulletHit.bulletFalling = false;
                isReloading = true;
                StopAllCoroutines();
                StartCoroutine(StartReload());
            }
        }
    }

    private IEnumerator StartReload()
    {
        SetAnimation(AnimationState.stunStart, Reloading);
        AnimationReference animationRefAsset;
        animationStateRef.TryGetValue(AnimationState.stunEnd, out animationRefAsset);
        yield return new WaitForSeconds(reloadTime - animationRefAsset.speed);
        SetAnimation(AnimationState.stunEnd);
        yield return new WaitForSeconds(animationRefAsset.speed);
        isReloading = false;
        SetAnimation(AnimationState.idle);
    }

    private void Reloading(TrackEntry trackEntry)
    {
        SetAnimation(AnimationState.stun);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            player.GetComponent<Respawn>().RespawnPlayer();
    }
}
