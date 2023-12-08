using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Modifie les mouvements")]
    [SerializeField] private float speed = 5f;

    [Header("Modifie le saut")]
    [SerializeField] private float gravityFactor = 1f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Modifie le temps où le joueur saute après avoir quitté une plateforme")]
    [SerializeField] private float hangTime = 0.1f;
    
    [Header("Modifie le temps où l'input de saut a été enregistré")]
    [SerializeField] private float jumpBufferLength = 0.1f; 
    
    [Header("Modifie la rapidité pour tomber du saut")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    
    [Header("Component à set up")]
    [SerializeField] private Rigidbody2D playerRigidbody2D;

    [SerializeField] private RaycastDetection _raycastDetection;

    [Header("Ne pas set up")]
    [SerializeField] private float hangTimeCounter; 
    [SerializeField] private float jumpBufferCounter; 
    
    private bool canJump => jumpBufferCounter >= 0 && hangTimeCounter >= 0;

    private Vector2 playerVelocity;

    private void Update()
    {
        CoyoteTime();
        JumpBuffer();
    }

    private void FixedUpdate()
    {
        ModularMovement();
        SetGravity();
        ComputeGravity();
        if (canJump) Jump();
        
        playerRigidbody2D.velocity = playerVelocity;
    }

    private void ModularMovement()
    {
        if (InputReader.instance.direction.x > 0 && !_raycastDetection.stopRight ||
            InputReader.instance.direction.x < 0 && !_raycastDetection.stopLeft)
        {
            playerVelocity.x = InputReader.instance.direction.x * speed;
        }
        else
        {
            playerVelocity.x = 0;
        }
    }

    private void SetGravity()
    {
        if (_raycastDetection.isGrounded)
        {
            playerVelocity.y = 0;
        }
        else
        {
            playerVelocity.y += Physics2D.gravity.y * Time.deltaTime * gravityFactor;
        }
    }
    
    private void Jump()
    {
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;
        
        playerVelocity.y = jumpForce;
    }

    private void ComputeGravity()
    {
        bool isFalling = playerVelocity.y < 0;
        bool isReleasingJump = playerVelocity.y > 0 && !InputReader.instance.jump;
        
        if (!isFalling && !isReleasingJump)
        {
            return;
        }

        float factor = isFalling ? fallMultiplier : lowJumpMultiplier;
        playerVelocity += Vector2.up * Physics2D.gravity.y * (factor - 1) * Time.deltaTime;
    }

    private void CoyoteTime()
    {
        if (_raycastDetection.isGrounded) hangTimeCounter = hangTime;
        else hangTimeCounter -= Time.deltaTime;
    }

    private void JumpBuffer()
    {
        if (InputReader.instance.jump) jumpBufferCounter = jumpBufferLength;
        else jumpBufferCounter -= Time.deltaTime;
    }
}