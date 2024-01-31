using UnityEngine;
using UnityEngine.InputSystem;

public class FootTriggerPlatform : MonoBehaviour
{
    private Vector2 direction;
    private BoxCollider2D otherBC2D;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherBC2D = other.GetComponent<BoxCollider2D>();
            otherBC2D.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherBC2D.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (direction.y < 0 && otherBC2D != null)
        {
            otherBC2D.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    public void UpInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }
}
