using System.Collections;
using UnityEngine;

public class IARampant : IA
{
    [SerializeField] private float distanceXtoFall = 1f, IaStunTime = 3f;
    private bool isOnGround;
    protected RaycastHit2D RaycastDetectNotVoidRampant
    {
        get
        {
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * tailleMob.x, Vector2.up * tailleMob.y);
            return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * tailleMob.x, Vector2.up, tailleMob.y * 1.5f, layerDefault);
        }
    }
    private State state;
    private enum State
    {
        WaitPlayer,
        ChasePlayer,
    }

    protected override void Update()
    {
        AtkPlayer();
        if (isOnGround && !IsGrounded) return;
        StateManager();
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
        if (DetectPlayer)
        {
            state = State.ChasePlayer;
            direction = transform.position.x < player.position.x;
            currentSpeedMovement = speedAttaquePlayer;
        }
    }

    private void ChasePlayer()
    {
        direction = transform.position.x < player.position.x;
        if (!isOnGround && !RaycastDetectNotVoidRampant) return;
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
                currentSpeedMovement = 0f;
                yield return new WaitForSeconds(IaStunTime);
                currentSpeedMovement = speedAttaquePlayer;
            }
        }
        else if (!DetectPlayer)
        {
            state = State.WaitPlayer;
            rb2D.velocity = Vector2.zero;
        }
    }
}
