using UnityEngine;

public class OldBetterPlayerJump : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    
    [SerializeField] private Rigidbody2D playerRigidbody2D;

    private void Update()
    {
        if (playerRigidbody2D.velocity.y < 0)
        {
            playerRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRigidbody2D.velocity.y > 0 && !InputReader.instance.jump)
        {
            playerRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}