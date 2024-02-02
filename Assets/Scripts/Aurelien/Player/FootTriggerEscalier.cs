using UnityEngine;
using UnityEngine.InputSystem;

public class FootTriggerEscalier : MonoBehaviour
{
    private BoxCollider2D bc2D;
    private bool triggerActive, onPlatform;
    private Vector2 direction;

    private void Start()
    {
        bc2D = GetComponent<BoxCollider2D>();
    }    

    public void UpInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        // if (!context.started) return;
        if (direction.y > 0)
        {
            triggerActive = true;
            bc2D.enabled = true;
        }
        else
        {
            triggerActive = false;
            if (!onPlatform)
                bc2D.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Escalier"))
        {
            onPlatform = true;
            other.GetComponent<Escalier>().OnEscalier(transform.parent, true, transform.localPosition.y);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Escalier"))
        {
            if (!triggerActive)
                bc2D.enabled = false;
            onPlatform = false;
            other.GetComponent<Escalier>().OnEscalier(null, false, 0f);
        }
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (onPlatform)
        {
            triggerActive = false;
            bc2D.enabled = false;
        }
        if (direction.y < 0)
            InputReader.instance.DontJump = true;
        else
            InputReader.instance.DontJump = false;
    }
}
