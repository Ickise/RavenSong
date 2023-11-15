using System.Collections;
using UnityEngine;

public abstract class IA : MonoBehaviour
{
    protected Rigidbody2D rb2D;
    protected Transform player;
    protected LayerMask layerDefault, layerDetectPlayer;
    [Tooltip("direction at the start"), SerializeField] protected bool direction; //left = false, right = true
    [SerializeField] protected float distanceIsGrounded = 1.1f, distanceHitSomething = 0.6f, speedRoaming = 2f, speedChasingPlayer = 3f, detectPlayerRange = 10f, jumpForce = 10f;
    [SerializeField] private bool canJumpObstacle;
    protected float speedMovement;
    protected bool canJump = true;
    protected RaycastHit2D RaycastDetectNotVoid
    {
        get
        {
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * distanceHitSomething, Vector2.down * 2f);
            return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * distanceHitSomething, Vector2.down, distanceIsGrounded * 2f, layerDefault);
        }
    }
    protected RaycastHit2D IsGrounded { get { return Physics2D.Raycast(transform.position, Vector2.down, distanceIsGrounded, layerDefault); } }
    protected RaycastHit2D RaycastHitWall { get { return Physics2D.Raycast(transform.position, direction ? Vector2.right : Vector2.left, distanceHitSomething, layerDefault); } }
    protected bool DetectPlayerX { get { return DistanceBetweenIAandPlayer < detectPlayerRange && Mathf.Abs(transform.position.y - player.position.y) < 2f && RaycastDetectPlayer.transform.CompareTag("Player"); } }
    protected float DistanceBetweenIAandPlayer { get { return Vector2.Distance(player.position, transform.position); } }
    protected RaycastHit2D RaycastDetectPlayer { get { return Physics2D.Raycast(transform.position, player.position - transform.position, detectPlayerRange, layerDetectPlayer); } }

    void Start()
    {
        speedMovement = speedRoaming;
        layerDefault = LayerMask.GetMask("Default") | LayerMask.GetMask("Props");
        layerDetectPlayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Player") | LayerMask.GetMask("Props");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        StateManager();
        // if (canJumpObstacle)
        //     JumpObstacle();
        // print(IsGrounded + "   " + !RaycastHitWall + "   " + Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * distanceHitSomething, Vector2.down, distanceIsGrounded, layerDefault));
    }

    // private void JumpObstacle()
    // {
    //     if (IsGrounded && !RaycastHitWall && Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * distanceHitSomething, Vector2.down, distanceIsGrounded, layerDefault))
    //         rb2D.AddForce((direction ? Vector2.one : new Vector2(-1, 1)) * 2f, ForceMode2D.Impulse);
    // }

    protected abstract void StateManager();

    protected void RunToDirection()
    {
        rb2D.velocity = new Vector2(direction ? speedMovement : -speedMovement, rb2D.velocity.y);
    }

    // private void Roaming()
    // {
    //     rb2D.velocity = new Vector2(direction ? speedMovement : -speedMovement, rb2D.velocity.y);
    //     if (Physics2D.Raycast(transform.position, direction ? Vector2.right : Vector2.left, distanceHitSomething, layerDefault))
    //     {
    //         if (canJumpWall && !canJump)
    //         {
    //             if (!Physics2D.Raycast(transform.position + new Vector3(0, jumpForce / 5, 0), direction ? Vector2.right : Vector2.left, distanceHitSomething * 2))
    //             {
    //                 rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    //                 canJump = true;
    //                 StartCoroutine(InJumpWall());
    //                 IEnumerator InJumpWall() { yield return new WaitForSeconds(1f); canJump = false; }
    //             }
    //             else
    //                 direction = !direction;
    //         }
    //         else if (!canJump)
    //             direction = !direction;
    //     }
    //     if (canJumpWall)
    //         Debug.DrawRay(transform.position + new Vector3(0, jumpForce / 5, 0), (direction ? Vector2.right : Vector2.left) * distanceHitSomething * 2, Color.green);
    //     Debug.DrawRay(transform.position, distanceHitSomething * (direction ? Vector2.right : Vector2.left), Color.green);
    // }
}
