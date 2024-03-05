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
            ChangeOtherCollider(false,"Ground" );
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            ChangeOtherCollider(true,"Ignore Raycast" );
            otherC2D = null;
        }
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (InputReader.instance.direction.y < 0 && otherC2D != null)
        {
            ChangeOtherCollider(true,"Ignore Raycast" );
        }
    }
    
    private void ChangeOtherCollider(bool isTrigger, string nameOfLayer)
    {
        otherC2D.isTrigger = isTrigger;
        otherC2D.gameObject.layer = LayerMask.NameToLayer(nameOfLayer);
    }
}

/*using UnityEngine;
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
            ChangeOtherCollider(true, "Ignore Raycast");
            otherC2D = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D = other.GetComponent<Collider2D>();
            ChangeOtherCollider(false, "Ground");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D = other.GetComponent<Collider2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Platforme"))
        {
            otherC2D = other.GetComponent<Collider2D>();
            ChangeOtherCollider(true, "Ignore Raycast");
            otherC2D = null;
        }
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (direction.y < 0 && otherC2D != null)
        {
            ChangeOtherCollider(true, "Ignore Raycast");
        }
    }

    public void UpInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void ChangeOtherCollider(bool isTrigger, string nameOfLayer)
    {
        otherC2D.isTrigger = isTrigger;
        otherC2D.gameObject.layer = LayerMask.NameToLayer(nameOfLayer);
    }
}*/
