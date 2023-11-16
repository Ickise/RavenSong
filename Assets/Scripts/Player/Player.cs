using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

//ce script gère les mouvements de base des personnages, donc déplacement et jump
public class Player : MonoBehaviour
{
    private Vector2 direction;
    private SpriteRenderer spriteRenderer;
    private LayerMask layerDefault, layerIA, layerCanJumpOn;
    private Rigidbody2D rigidBody2D;
    private float puissanceJump, accelerationSpeedCharacter, airControlSpeed, gravityPower, maxSpeedCharacter;
    [SerializeField] private int nbDoubleJump;
    [SerializeField] private float distanceRoulade, speedRoulade;
    private int currentNbJump;
    private float lastDirection;
    private bool IsGrounded
    {
        get
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, 1.1f, layerCanJumpOn))
            {
                currentNbJump = nbDoubleJump;
                return true;
            }
            return false;
        }
    }
    private bool[] IsOnWall
    {
        get
        {
            return new bool[]
            {
                Physics2D.Raycast(transform.position, Vector2.right, 0.8f, layerDefault),
                Physics2D.Raycast(transform.position, Vector2.left, 0.8f, layerDefault)
            };
        }
    }
    private bool IsOnWalls
    {
        get
        {
            if (IsOnWall[0] || IsOnWall[1])
            {
                currentNbJump = nbDoubleJump;
                return IsOnWall[0] || IsOnWall[1];
            }
            return false;
        }
    }

    void Start()
    {
        lastDirection = 1f;
        layerDefault = LayerMask.GetMask("Default");
        layerCanJumpOn = LayerMask.GetMask("Default") | LayerMask.GetMask("Props");
        layerIA = LayerMask.GetMask("IA");
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        puissanceJump = 20;
        accelerationSpeedCharacter = 5;
        maxSpeedCharacter = 10;
        airControlSpeed = 0.2f;
        gravityPower = -1.5f;
    }

    public void GetInputsDeplacement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        if (context.performed)
            lastDirection = direction.x;
    }

    void Update()
    {
        if (DOTween.IsTweening("roll") && (!IsGrounded || IsOnWalls))
            DOTween.Kill("roll");
        Debug.DrawRay(transform.position, Vector2.left * 0.8f, Color.cyan);
    }

    void FixedUpdate()
    {
        if (DOTween.IsTweening(transform)) return;
        Deplacements();
        Debug.DrawRay(transform.position, Vector2.down * 1.1f, Color.green);
    }

    private void Deplacements()
    {
        rigidBody2D.velocity += new Vector2((IsGrounded ? direction.x : direction.x * airControlSpeed) * accelerationSpeedCharacter, IsOnWalls ? gravityPower / 2 : gravityPower);
        rigidBody2D.velocity = new Vector2(Mathf.Clamp(rigidBody2D.velocity.x, -maxSpeedCharacter, maxSpeedCharacter), rigidBody2D.velocity.y);
        if (IsGrounded && direction.x == 0)
            rigidBody2D.velocity = new Vector2(Mathf.Lerp(rigidBody2D.velocity.x, 0, 0.25f), rigidBody2D.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (DOTween.IsTweening(transform) || !context.started) return;
        if (!IsOnWalls && currentNbJump > 0 || IsGrounded)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);
            rigidBody2D.AddForce(Vector2.up * puissanceJump, ForceMode2D.Impulse);
            currentNbJump--;
        }
        else if (IsOnWalls)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, rigidBody2D.velocity.y / 2);
            rigidBody2D.AddForce((IsOnWall[0] ? new Vector2(-1, 1).normalized : new Vector2(1, 1).normalized) * puissanceJump, ForceMode2D.Impulse);
        }
    }

    public void Roll(InputAction.CallbackContext context)
    {
        if (DOTween.IsTweening(transform) || !context.started || !IsGrounded) return;
        transform.localScale += Vector3.down * 0.5f;
        transform.position += Vector3.down * 0.5f;
        rigidBody2D.DOMoveX(transform.position.x + distanceRoulade * lastDirection, speedRoulade).SetId("roll").SetSpeedBased(true)
        .OnComplete(() =>
        {
            transform.DOScaleY(1f, 0.2f);
        }).OnKill(() =>
        {
            if (IsOnWalls)
            {
                rigidBody2D.velocity = Vector2.zero;
                rigidBody2D.AddForce((IsOnWall[0] ? new Vector2(-1, 1).normalized : new Vector2(1, 1).normalized) * (puissanceJump / 2), ForceMode2D.Impulse);
            }
            transform.DOMoveY(transform.position.y + 0.5f, 0.1f);
            transform.DOScaleY(1f, 0.1f);
        });
    }


    public void CrossHit(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position + Vector3.down * 0.5f, Vector2.right * lastDirection, 1f, layerIA);
        if (hit2D.transform != null && hit2D.transform.gameObject.layer == LayerMask.NameToLayer("IA"))
            hit2D.transform.DOShakePosition(3f, Vector3.right * 0.1f, 30, 0, false, false, ShakeRandomnessMode.Full);
        Debug.DrawRay(transform.position + Vector3.down * 0.5f, Vector2.right * lastDirection, Color.green, 1f);
    }
}

