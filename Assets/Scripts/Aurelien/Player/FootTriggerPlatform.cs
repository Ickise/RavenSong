using UnityEngine;
using UnityEngine.InputSystem;

public class FootTriggerPlatform : MonoBehaviour
{
    private Vector2 direction;
    private Collider2D otherC2D;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D = other.GetComponent<Collider2D>();
            otherC2D.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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
