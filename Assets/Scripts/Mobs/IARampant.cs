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
        if (RaycastDetectPlayer.transform.CompareTag("Player"))
        {
            state = State.ChasePlayer;
            direction = transform.position.x < player.position.x;
            speedMovement = speedChasingPlayer;
        }
    }

    private void ChasePlayer()
    {
        direction = transform.position.x < player.position.x;
        rb2D.velocity = new Vector2(direction ? speedMovement : -speedMovement, rb2D.velocity.y);
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
                speedMovement = speedChasingPlayer;
            }
        }
        else if (!RaycastDetectPlayer.transform.CompareTag("Player"))
        {
            state = State.WaitPlayer;
            rb2D.velocity = Vector2.zero;
        }
    }
}
