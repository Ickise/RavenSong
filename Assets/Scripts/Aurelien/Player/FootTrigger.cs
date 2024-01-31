using UnityEngine;
using UnityEngine.InputSystem;

public class FootTrigger : MonoBehaviour
{
    private BoxCollider2D bc2D;
    private bool triggerActive;

    private void Start()
    {
        bc2D = GetComponent<BoxCollider2D>();
    }

    public void UpInput(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>() == Vector2.up)
            bc2D.enabled = true;
        // else
            
    }
}
