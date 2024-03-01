using UnityEngine;
using UnityEngine.InputSystem;

public class FootTriggerPlatform : MonoBehaviour
{
    private Vector2 direction;
    private Collider2D otherC2D;

    private bool canFallOfPlatform;
    
    private void Update()
    {
        canFallOfPlatform = InputReader.instance.canDown && InputReader.instance.jump;
        
        if (canFallOfPlatform && otherC2D != null)
        {
            otherC2D.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            otherC2D.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D = other.GetComponent<Collider2D>();
            otherC2D.isTrigger = false;
            otherC2D.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D = other.GetComponent<Collider2D>();
            otherC2D.isTrigger = false;
            otherC2D.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D = other.GetComponent<Collider2D>();
            otherC2D.isTrigger = true;
            otherC2D.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            otherC2D = null;
        }
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (direction.y < 0 && otherC2D != null)
        {
            otherC2D.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    public void UpInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }
}
