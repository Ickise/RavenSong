using System.Collections;
using UnityEngine;
using DG.Tweening;

public class IAHorloger : IA
{
    [SerializeField] private float reloadTime = 5f, runTime = 5f, decelerationTime = 1f;
    [SerializeField] private int chargeTime = 1;
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
        tailleMob.y += 0.2f;
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
            DOTween.To(() => a, x => a = x, 1f, chargeTime).SetId("chargingTime")
            // ;spriteRenderer.DOColor(Color.red, chargeTime / 4f)
            .OnComplete(() => { state = State.ChasePlayer; StartCoroutine(RunTime()); });
            IEnumerator RunTime()
            {
                yield return new WaitForSeconds(runTime);
                DOTween.To(() => rb2D.velocity, x => rb2D.velocity = x, Vector2.zero, decelerationTime);
                isReloading = true;
                StartCoroutine(StartReload());
                state = State.WaitPlayer;
            }
        }
    }

    private void ChasePlayer()
    {
        rb2D.velocity = new Vector2(direction ? speedAttaquePlayer : -speedAttaquePlayer, rb2D.velocity.y);
        transform.localScale = direction ? Vector2.one : new Vector2(-1, 1);
    }

    private void IsChasePlayer()
    {
        if (RaycastHitWall)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce((direction ? new Vector2(-1, 1) : Vector2.one) * 5f, ForceMode2D.Impulse);
            state = State.WaitPlayer;
            isReloading = true;
            StopAllCoroutines();
            StartCoroutine(StartReload());
        }
    }

    IEnumerator StartReload()
    {
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }
}
