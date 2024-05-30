using System.Collections;
using UnityEngine;

public class IAProjectil : IA
{
    [SerializeField] private float minDistanceToAttack = 5f, maxDistanceToAttack = 20f, timeBetweenAttack = 3f;
    private float minPauseTime = 3f, maxPauseTime = 6f, minTimeBetweenPause = 6f, maxTimeBetweenPause = 12f;
    [Header("VFX")]
    [SerializeField] private GameObject projectil, VFXTir;
    [SerializeField] private Transform posVFXTir;
    [SerializeField, Header("Sounds")] private SoundData mort, rire, detection, idle;
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
        currentSpeedMovement = speedBalader;
        if (RaycastHitWall || !RaycastDetectNotVoid)
            direction = !direction;
        RunToDirection();
    }

    private void IsRoaming()
    {
        if (Mathf.RoundToInt(Random.Range(0, 30 / Time.deltaTime)) == 0)
            AudioManager.instance.PlaySound(idle);
        if (DetectPlayer || DetectPlayerYProjectil)
        {
            state = State.RushDistancePlayer;
            AudioManager.instance.PlaySound(detection);
            currentSpeedMovement = speedAttaquePlayer;
        }
    }

    private void RushDistancePlayer()
    {
        float posATK = player.position.x + (transform.position.x > player.position.x ? (minDistanceToAttack + maxDistanceToAttack) / 2f : -((minDistanceToAttack + maxDistanceToAttack) / 2f));
        direction = transform.position.x < posATK;
        if (Mathf.Abs(posATK - transform.position.x) < (minDistanceToAttack + maxDistanceToAttack) / 2f)
        {
            state = State.Attack;
            StartCoroutine(Attack());
        }
        else if (RaycastHitWall || !RaycastDetectNotVoid)
        {
            if (!DetectPlayer && !DetectPlayerYProjectil)
            {
                state = State.Roaming;
                return;
            }
            rb2D.velocity = Vector2.zero;
            return;
        }
        RunToDirection();
    }

    private void IsAttacking()
    {
        float posATK = player.position.x + (transform.position.x > player.position.x ? (minDistanceToAttack + maxDistanceToAttack) / 2f : -((minDistanceToAttack + maxDistanceToAttack) / 2f));
        direction = transform.position.x < posATK;
        if (Mathf.Abs(transform.position.x - player.position.x) < minDistanceToAttack || Mathf.Abs(transform.position.x - player.position.x) > maxDistanceToAttack || (!DetectPlayer && !DetectPlayerYProjectil))
        {
            if ((RaycastHitWall || !RaycastDetectNotVoid) && Mathf.Abs(transform.position.x - player.position.x) < minDistanceToAttack && (DetectPlayerYProjectil || DetectPlayer))
            {
                rb2D.velocity = Vector2.zero;
                return;
            }
            StopAllCoroutines();
            state = State.Roaming;
            return;
        }
        if (Mathf.Abs(posATK - transform.position.x) < 0.2f)
        {
            SetAnimation(AnimationState.idle);
            rb2D.velocity = Vector2.zero;
            return;
        }

        if (transform.position.x > player.position.x)
        {
            if (direction)
                RunToDirection(AnimationState.moveBackward, true);
            else
                RunToDirection();
        }
        else
        {
            if (!direction)
                RunToDirection(AnimationState.moveBackward, true);
            else
                RunToDirection();
        }
    }

    private IEnumerator Attack()
    {
        transform.localScale = transform.position.x < player.position.x ? Vector2.one : new Vector2(-1, 1);
        yield return new WaitForSeconds(timeBetweenAttack);
        SetAnimation(rb2D.velocity == Vector2.zero ? AnimationState.shoot : AnimationState.shootWalk);
        Instantiate(projectil, transform.position, Quaternion.identity).GetComponent<Projectil>().playerPos = player.position;
        GameObject currentVFXTir = Instantiate(VFXTir, posVFXTir.position, Quaternion.identity);
        Destroy(currentVFXTir, 3);
        StartCoroutine(Attack());
    }

    private void OnDisable()
    {
        state = State.Roaming;
        rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    private void OnDestroy()
    {
        AudioManager.instance.PlaySound(mort);
    }
}
