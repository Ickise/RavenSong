using UnityEngine;
using UnityEngine.InputSystem;

public class FootTriggerPlatform : MonoBehaviour
{
    private Collider2D otherC2D;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D = other.GetComponent<Collider2D>();
            otherC2D.isTrigger = false;
            otherC2D.gameObject.layer = LayerMask.NameToLayer("Ground");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D.isTrigger = true;
            otherC2D.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            otherC2D = null;
        }
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (InputReader.instance.direction.y < 0 && otherC2D != null)
        {
            print("zzef");
            otherC2D.isTrigger = true;
            otherC2D.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
}
