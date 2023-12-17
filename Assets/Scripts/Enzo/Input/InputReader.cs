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
    public bool canRoll;

    public static InputReader instance;
    private PlayerController2D _playerController2D;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        instance = this;
        _playerController2D = GetComponent<PlayerController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();

        if (context.performed)
            _playerController2D.LastDirection = spriteRenderer.flipX ? -1f : 1f;
        else if (context.started && direction.x != 0)
            spriteRenderer.flipX = direction.x < 0;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jump = true;
            _playerController2D.SetVelocity();
        }
        if (context.canceled) jump = false;
    }

    public void OnFire(InputAction.CallbackContext context) => leftClick = context.performed;

    public void OnAim(InputAction.CallbackContext context) => activateAim = context.performed;

    public void OnStun(InputAction.CallbackContext context) => canStun = context.performed;

    public void OnDown(InputAction.CallbackContext context) => canDown = context.performed;

    public void OnRoll(InputAction.CallbackContext context) { if (context.started) _playerController2D.Roll(); }
}