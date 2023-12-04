using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [Header("Ne pas set up")]
    public Vector2 direction;

    public bool jump;
    public bool leftClick;
    public bool activateAim = false;
    public bool canStun;
    public bool canDown;
    
    public static InputReader instance;
    
    private void Start()
    {
        instance = this;
    }
    
    public void OnMovement(InputAction.CallbackContext context) => direction = context.ReadValue<Vector2>();
    
    public void OnJump(InputAction.CallbackContext context) => jump = context.performed;

    public void OnFire(InputAction.CallbackContext context) => leftClick = context.performed;
    
    public void OnAim(InputAction.CallbackContext context) => activateAim = context.performed;

    public void OnStun(InputAction.CallbackContext context) => canStun = context.performed;

    public void OnDown(InputAction.CallbackContext context) => canDown = context.performed;
}