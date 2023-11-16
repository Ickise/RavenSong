using System.Collections;
using UnityEngine;

public class IAClock : IA
{
    [SerializeField] private float reloadTime;
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
        if (DetectPlayerY)
        {
            state = State.ChasePlayer;
            direction = transform.position.x < player.position.x;
        }
    }

    private void ChasePlayer()
    {
        rb2D.velocity = new Vector2(direction ? speedChasingPlayer : -speedChasingPlayer, rb2D.velocity.y);
    }

    private void IsChasePlayer()
    {
        if (Physics2D.Raycast(transform.position, direction ? Vector2.right : Vector2.left, distanceHitSomething, layerDetectPlayer))
        {
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce((direction ? new Vector2(-1, 1) : Vector2.one) * 5f, ForceMode2D.Impulse);
            state = State.WaitPlayer;
            isReloading = true;
            StartCoroutine(StartReload());
            IEnumerator StartReload()
            {
                yield return new WaitForSeconds(reloadTime);
                isReloading = false;
            }
        }
    }
}
