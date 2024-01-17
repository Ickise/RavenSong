using System.Collections;
using UnityEngine;

public class IAProjectil : IA
{
    [SerializeField] private float minDistance = 5f, maxDistance = 20f, timeAttack = 3, minPauseTime = 3f, maxPauseTime = 6f, minTimeBetweenPause = 6f, maxTimeBetweenPause = 12f;
    [SerializeField] private GameObject projectil;
    private RaycastHit2D RaycastDetectPlayerProjectil
    {
        get
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.up, hauteurPlayerDetection, layerDefault);
            return hit2D ? Physics2D.Raycast(new Vector2(hit2D.point.x, hit2D.point.y - 0.1f), player.position - new Vector3(hit2D.point.x, hit2D.point.y - 0.1f), distancePlayerDetection, layerDetectPlayer) :
            Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + hauteurPlayerDetection), player.position - new Vector3(transform.position.x, transform.position.y + hauteurPlayerDetection), distancePlayerDetection, layerDetectPlayer);
        }
    }
    private bool DetectPlayerYProjectil { get { return Mathf.Abs(transform.position.y - player.position.y) < hauteurPlayerDetection && RaycastDetectPlayerProjectil && RaycastDetectPlayerProjectil.transform.CompareTag("Player"); } }

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
        if (RaycastHitWall || !RaycastDetectNotVoid)
            direction = !direction;
        RunToDirection();
    }

    private void IsRoaming()
    {
        if (DetectPlayer || DetectPlayerYProjectil)
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
        if (Mathf.Abs(transform.position.x - player.position.x) < minDistance || Mathf.Abs(transform.position.x - player.position.x) > maxDistance || !DetectPlayerYProjectil)
        {
            if ((Mathf.Abs(transform.position.x - player.position.x) < minDistance || Mathf.Abs(transform.position.x - player.position.x) > maxDistance) && (RaycastHitWall || !RaycastDetectNotVoid) && (DetectPlayerYProjectil || DetectPlayer))
                return;
            StopAllCoroutines();
            state = State.Roaming;
        }
    }

    private IEnumerator Attack()
    {
        spriteRenderer.flipX = transform.position.x < player.position.x;
        yield return new WaitForSeconds(timeAttack);
        Instantiate(projectil, transform.position, Quaternion.identity);
        StartCoroutine(Attack());
    }
}
