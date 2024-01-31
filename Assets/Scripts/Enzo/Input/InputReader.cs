using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [Header("Ne pas set up")] public Vector2 direction;

    public bool jump;
    public bool leftClick;
    public bool activateAim = false;
    public bool canStun;
    public bool canDown;
    public bool canRoll;
    public bool DontJump { private get; set; }

    public UnityEvent onInteractionEvent = new UnityEvent();

    public static InputReader instance;
    private PlayerController2D _playerController2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        instance = this;
        _playerController2D = GetComponent<PlayerController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        if (context.performed)
        {
            AnimationController.instance.FlipAnimation(direction.x > 0);
            _playerController2D.LastDirection = AnimationController.instance.GetDirection ? 1f : -1f;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (DontJump) return;
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

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started) _playerController2D.Roll();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onInteractionEvent.Invoke();
        }
    }
}