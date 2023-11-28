using System.Collections;
using UnityEngine;

public class IARampant : IA
{
    [SerializeField] private float distanceXtoFall = 1f, IaStunTime = 3f;
    private bool isOnGround;
    private State state;
    private enum State
    {
        WaitPlayer,
        ChasePlayer,
    }

    protected override void StateManager()
    {
        if (isOnGround && !IsGrounded) return;
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
        if (DetectPlayer)
        {
            state = State.ChasePlayer;
            direction = transform.position.x < player.position.x;
            speedMovement = speedAttaquePlayer;
        }
    }

    private void ChasePlayer()
    {
        direction = transform.position.x < player.position.x;
        RunToDirection();
    }

    private void IsChasePlayer()
    {
        if (!isOnGround && Mathf.Abs(transform.position.x - player.position.x) < distanceXtoFall)
        {
            rb2D.gravityScale *= -1f;
            isOnGround = true;
            StartCoroutine(StunTime());
            IEnumerator StunTime()
            {
                speedMovement = 0f;
                yield return new WaitForSeconds(IaStunTime);
                speedMovement = speedAttaquePlayer;
            }
        }
        else if (!DetectPlayer)
        {
            state = State.WaitPlayer;
            rb2D.velocity = Vector2.zero;
        }
    }
}
