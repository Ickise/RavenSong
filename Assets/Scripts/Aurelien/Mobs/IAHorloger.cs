using System.Collections;
using UnityEngine;
using DG.Tweening;

public class IAHorloger : IA
{
    [SerializeField] private float reloadTime = 5f, runTime = 5f, decelerationTime = 1f;
    private bool isReloading;
    private State state;
    private enum State
    {
        WaitPlayer,
        ChasePlayer
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
        if (isReloading) return;
        if (DetectPlayer)
        {
            state = State.ChasePlayer;
            direction = transform.position.x < player.position.x;
            StartCoroutine(RunTime());
            IEnumerator RunTime()
            {
                yield return new WaitForSeconds(runTime);
                DOTween.To(() => rb2D.velocity, x => rb2D.velocity = x, Vector2.zero, decelerationTime);
                StartCoroutine(StartReload());
                state = State.WaitPlayer;
            }
        }
    }

    private void ChasePlayer()
    {
        rb2D.velocity = new Vector2(direction ? speedAttaquePlayer : -speedAttaquePlayer, rb2D.velocity.y);
    }

    private void IsChasePlayer()
    {
        if (Physics2D.Raycast(transform.position, direction ? Vector2.right : Vector2.left, tailleMob.x, layerDetectPlayer))
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
