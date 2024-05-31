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
    [Header("VFX")]
    [SerializeField] private GameObject VFXBonk, VFXImpactMetalBall, VFXCourse, VFXCourseEtincel;
    [SerializeField] private Transform posVFXBonk, posVFXImpactMetalBall, posVFXCourse, posVFXCourseEtincel;
    [Header("Sounds")]
    [SerializeField] private SoundData mort, charge, detection, idle, recharge;
    private MeshRenderer currentShaderBrillance;
    private float currentValueShaderBrillance;
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
        ShaderBrillance(1f, reloadTime);
    }

    protected override void Update()
    {
        base.Update();
        Color sbc = currentShaderBrillance.material.GetColor("_Flash_Fatigue_Color");
        currentShaderBrillance.material.SetColor("_Flash_Fatigue_Color", new Color(sbc.r, currentValueShaderBrillance, sbc.b, sbc.a));
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
        if (Mathf.RoundToInt(Random.Range(0, 30 / Time.deltaTime)) == 0)
            AudioManager.instance.PlaySound(idle);
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
                ShaderBrillance(0, runTime);
                AudioManager.instance.PlaySound(charge);
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
                AudioManager.instance.PlaySound(recharge);
                state = State.WaitPlayer;
                ShaderBrillance(1f, reloadTime);
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
                    Physics2D.IgnoreCollision(RaycastHitWall.transform.GetChild(0).GetComponent<Collider2D>(), cd2D);
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
                AudioManager.instance.PlaySound(recharge);
                ShaderBrillance(1f, reloadTime);
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

    public void PlayVFXImpactMetalBall()
    {
        VFXInstantieur.instance.PlayVFXInWorld(VFXImpactMetalBall, posVFXImpactMetalBall);
    }

    private void ShaderBrillance(float gValue, float time)
    {
        currentShaderBrillance = skeletonAnimation.transform.GetComponent<MeshRenderer>();
        DOTween.To(() => currentValueShaderBrillance, x => currentValueShaderBrillance = x, gValue, time)
        .SetEase(Ease.OutBounce);
    }

    private void OnDestroy()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySound(mort);
    }
}
