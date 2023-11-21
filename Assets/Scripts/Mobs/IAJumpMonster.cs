using System.Collections;
using UnityEngine;

public class IAJumpMonster : IA
{
    [SerializeField] private float timeWaitingForJumpRoaming, timeWaitingForJumpAtk;
    private float timeWaitingForJump;

    protected override void StateManager()
    {
        Roaming();
        DetectionPlayer();
    }

    private void Roaming()
    {
        if (canJump && Physics2D.Raycast(transform.position, Vector2.down, tailleMob, layerDefault))
        {
            rb2D.AddForce((direction ? Vector2.one : new Vector2(-1, 1)) * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            StartCoroutine(TimeWaitingForJump());
            IEnumerator TimeWaitingForJump()
            {
                yield return new WaitForSeconds(timeWaitingForJump);
                canJump = true;
            }
        }
        Debug.DrawRay(transform.position, Vector2.down * tailleMob);

        if (RaycastHitWall)
            direction = !direction;
        Debug.DrawRay(transform.position, (direction ? Vector2.right : Vector2.left) * distanceToucheMurOuVide);
    }

    private void DetectionPlayer()
    {
        if (DetectPlayerY)
        {
            direction = transform.position.x < player.position.x;
            timeWaitingForJump = timeWaitingForJumpAtk;
        }
        else 
            timeWaitingForJump = timeWaitingForJumpRoaming;
    }
}
