using UnityEngine;

public abstract class IA : MonoBehaviour
{
    protected Rigidbody2D rb2D;
    protected Transform player;
    protected SpriteRenderer spriteRenderer;
    protected LayerMask layerDefault, layerDetectPlayer;
    [Tooltip("direction au start"), SerializeField] protected bool direction; //left = false, right = true
    [SerializeField] protected float speedBalader = 2f, speedAttaquePlayer = 3f, distancePlayerDetection = 10f, hauteurPlayerDetection = 2f, jumpForce = 10f, distanceAttaquePlayer = 1f;
    [SerializeField] protected Vector2 tailleMob = new Vector2(1f, 2f);
    [SerializeField] private bool drawCirclesEditor;
    [SerializeField] private int nombreVie = 1;
    public int NbVie { get { return nombreVie; } set { nombreVie = value; } }
    protected float speedMovement;
    protected bool canJump = true;
    protected RaycastHit2D RaycastDetectNotVoid
    {
        get
        {
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * tailleMob.x, Vector2.down * tailleMob.y);
            return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * tailleMob.x, Vector2.down, tailleMob.y * 1.5f, layerDefault);
        }
    }
    protected RaycastHit2D IsGrounded { get { return Physics2D.Raycast(transform.position, Vector2.down, tailleMob.y, layerDefault); } }
    protected RaycastHit2D RaycastHitWall { get { return Physics2D.CapsuleCast(transform.position, new Vector2(0.1f, tailleMob.y - 0.1f), CapsuleDirection2D.Vertical, 0, direction ? Vector2.right : Vector2.left, tailleMob.x, layerDefault); } }
    protected bool DetectPlayer { get { return Mathf.Abs(transform.position.y - player.position.y) < hauteurPlayerDetection && RaycastDetectPlayer && RaycastDetectPlayer.transform.CompareTag("Player"); } }
    private RaycastHit2D RaycastDetectPlayer { get { return Physics2D.Raycast(transform.position, player.position - transform.position, distancePlayerDetection, layerDetectPlayer); } }

    void Start()
    {
        tailleMob.x *= 0.7f;
        tailleMob.y += 0.1f;
        speedMovement = speedBalader;
        layerDefault = LayerMask.GetMask("Default") | LayerMask.GetMask("IADontCollide");
        layerDetectPlayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Player") | LayerMask.GetMask("IADontCollide");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (!IsGrounded) return;
        StateManager();
        AtkPlayer();
    }

    protected abstract void StateManager();

    protected void AtkPlayer()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, player.position - transform.position, distanceAttaquePlayer, layerDetectPlayer);
        if (hit2D && hit2D.transform.CompareTag("Player"))
            player.GetComponent<Respawn>().RespawnPlayer();
    }

    protected void RunToDirection()
    {
        rb2D.velocity = new Vector2(direction ? speedMovement : -speedMovement, rb2D.velocity.y);
        spriteRenderer.flipX = direction ? true : false;
    }

    void OnDrawGizmos()
    {
        if (!drawCirclesEditor) return;
        Gizmos.DrawWireSphere(transform.position, distancePlayerDetection);
    }

    // private void Update()
    // {
    //     StateManager();
    //     // if (canJumpObstacle)
    //     //     JumpObstacle();
    //     // print(IsGrounded + "   " + !RaycastHitWall + "   " + Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * distanceHitSomething, Vector2.down, distanceIsGrounded, layerDefault));
    // }

    // private void JumpObstacle()
    // {
    //     if (IsGrounded && !RaycastHitWall && Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * distanceHitSomething, Vector2.down, distanceIsGrounded, layerDefault))
    //         rb2D.AddForce((direction ? Vector2.one : new Vector2(-1, 1)) * 2f, ForceMode2D.Impulse);
    // }

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
