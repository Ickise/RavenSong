using System.Collections;
using UnityEngine;

public class IAProjectil : IA
{
    [SerializeField] private float minDistance = 5f, maxDistance = 20f, timeAttack = 3;
    [SerializeField] private GameObject projectil;
    private State state;
    private enum State
    {
        Roaming,
        RushDistancePlayer,
        Attack
    }

    protected override void StateManager()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                IsRoaming();
                break;
            case State.RushDistancePlayer:
                RushDistancePlayer();
                break;
            case State.Attack:
                IsAttacking();
                break;
        }
    }

    private void Roaming()
    {
        speedMovement = speedBalader;
        if (IsGrounded && (RaycastHitWall || !RaycastDetectNotVoid))
            direction = !direction;
        RunToDirection();
    }

    private void IsRoaming()
    {
        if (DetectPlayerY)
        {
            state = State.RushDistancePlayer;
            speedMovement = speedAttaquePlayer;
        }
    }

    private void RushDistancePlayer()
    {
        float posATK = player.position.x + (transform.position.x > player.position.x ? Mathf.Lerp(minDistance, maxDistance, 0.5f) : -Mathf.Lerp(minDistance, maxDistance, 0.5f));
        direction = transform.position.x < posATK;
        if ((Mathf.Abs(posATK - transform.position.x) < 0.2f) || RaycastHitWall || !RaycastDetectNotVoid)
        {
            state = State.Attack;
            rb2D.velocity = Vector2.zero;
            StartCoroutine(Attack());
            return;
        }
        RunToDirection();
    }

    private void IsAttacking()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) < minDistance || Mathf.Abs(transform.position.x - player.position.x) > maxDistance || !DetectPlayerY)
        {
            if (Mathf.Abs(transform.position.x - player.position.x) < minDistance && (RaycastHitWall || !RaycastDetectNotVoid))
                return;
            StopAllCoroutines();
            state = State.Roaming;
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(timeAttack);
        Instantiate(projectil, transform.position, Quaternion.identity);
        StartCoroutine(Attack());
    }
}
