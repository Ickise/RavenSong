using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = 1f;

    [SerializeField] private Rigidbody2D playerRigidbody2D;

    [SerializeField] private RaycastDetection _raycastDetection;
    
    [SerializeField] private AudioClip runAudio;
    
    private void FixedUpdate()
    {
        ModularMovement();
    }

    private void ModularMovement()
    {
        
            playerRigidbody2D.velocity = new Vector2(InputReader.instance.direction.x * speed, playerRigidbody2D.velocity.y);
        

        if (_raycastDetection.isGrounded)
        {
            playerRigidbody2D.gravityScale = 0;
        }
        else
        {
            playerRigidbody2D.gravityScale = gravity;
        }
    }
}