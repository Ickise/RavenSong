using System.Collections;
using UnityEngine;

public class StunDetection : MonoBehaviour
{
    private Collider2D c2D;
    [Header("À set up")][SerializeField] private float stunDuration = 1.5f;
    [SerializeField] private float crossKickCooldown = 2f;
    [SerializeField] private float timeToDisableHitBox = 0.3f;
    private bool canCrossKick = true;

    private void Start()
    {
        c2D = GetComponent<Collider2D>();
    }

    public void CrossKick(int currentDirection)
    {
        if (!canCrossKick) return;
        AnimationController.instance.SetCharacterState(1, AnimationController.AnimationState.jump, false, AnimationController.instance.speedCrossKick, false);
        // AnimationController.instance.SetCharacterState(0, AnimationController.AnimationState.idleNoBall, true, AnimationController.instance.speedIdleBall, true);
        transform.localPosition = new Vector2(Mathf.Abs(transform.localPosition.x), transform.localPosition.y);
        transform.localPosition *= currentDirection;
        canCrossKick = false;
        StartCoroutine(CoolDown());
        IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(crossKickCooldown);
            canCrossKick = true;
        }
        c2D.enabled = true;
        StartCoroutine(TimeHitBox());
        IEnumerator TimeHitBox()
        {
            yield return new WaitForSeconds(stunDuration);
            c2D.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //pour les IA
        IA iA = other.GetComponent<IA>();
        if (iA)
        {
            iA.enabled = false;
            StartCoroutine(TimeUnstun());
            IEnumerator TimeUnstun()
            {
                yield return new WaitForSeconds(timeToDisableHitBox);
                iA.enabled = true;
            }
            return;
        }

        //pour les objets destructible
        Explodable destructibleObject = other.GetComponent<Explodable>();
        if (destructibleObject)
        {
            destructibleObject.explode();
            ExplosionForce ef = FindObjectOfType<ExplosionForce>();
            ef.doExplosion(transform.position);
            return;
        }

        //tu peux rajouter d'autre condition ici
    }

    // private void StunEnemy()
    // {
    //     timeToCrossKick += Time.deltaTime;

    //     if (InputReader.instance.canStun && timeToCrossKick >= crossKickCooldown)
    //     {
    //         timeToCrossKick = 0;

    //         canLaunchTimeToStun = true;
    //         stopTimeToEnableRaycast = true;

    //         raycastHit2D = Physics2D.Raycast(transform.position, Vector2.right * _playerController2D.LastDirection,
    //             distanceToHit, layerIa);

    //         if (raycastHit2D)
    //         {
    //             if (raycastHit2D.transform.GetComponent<IA>())
    //                 _ia = raycastHit2D.transform.GetComponent<IA>();
    //             else if (raycastHit2D.transform.CompareTag("DestroyObject"))
    //             {
    //                 Explodable explodableObj = raycastHit2D.transform.GetComponent<Explodable>();
    //                 explodableObj.explode();
    //                 ExplosionForce ef = FindObjectOfType<ExplosionForce>();
    //                 ef.doExplosion(transform.position);
    //             }
    //         }
    //     }

    //     if (canLaunchTimeToStun)
    //     {
    //         if (stopTimeToEnableRaycast) timeToEnableRaycast += Time.deltaTime;

    //         timeToStun += Time.deltaTime;

    //         if (_ia != null) _ia.enabled = false;

    //         if (timeToEnableRaycast >= timeToDisableRaycast)
    //         {
    //             raycastHit2D = new RaycastHit2D();

    //             timeToEnableRaycast = 0;
    //             stopTimeToEnableRaycast = false;
    //         }

    //         if (timeToStun >= stunDuration)
    //         {
    //             if (_ia != null)
    //             {
    //                 _ia.enabled = true;
    //                 _ia = null;
    //             }

    //             timeToStun = 0;
    //             canLaunchTimeToStun = false;
    //         }
    //     }
    // }
}