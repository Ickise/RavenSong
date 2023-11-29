using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float hangTime = 0.1f; //coyote time, temps après ne plus être sur une plateforme et qu'on puisse quand même sauter
    [SerializeField] private float hangTimeCounter; //le chrono pour pouvoir relancer un coyote time
    [SerializeField] private float jumpBufferLength = 0.1f; //jump buffer, correspond au moment où le joueur appuie sur espace pour sauter avant de toucher le sol
    [SerializeField] private float jumpBufferCounter; //chrono qui se lance pour pouvoir refaire un jumpbuffer

    private bool canJump => jumpBufferCounter > 0 && hangTimeCounter > 0;

    [SerializeField] private RaycastDetection _raycastDetection;

    [SerializeField] private Rigidbody2D playerRigidbody2D;
    
    [SerializeField] private AudioClip jumpAudio;
    
    private void Update()
    {
        if (canJump) Jump();

        if (InputReader.instance.jump) jumpBufferCounter = jumpBufferLength;
        else jumpBufferCounter -= Time.deltaTime;

        if (_raycastDetection.isGrounded) hangTimeCounter = hangTime;
        else hangTimeCounter -= Time.deltaTime;
    }

    private void Jump()
    {
        // AudioManager.instance.PlaySFX(jumpAudio);
        
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;
        
        playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, 0);
        playerRigidbody2D.velocity = Vector2.up * jumpForce;
    }
}