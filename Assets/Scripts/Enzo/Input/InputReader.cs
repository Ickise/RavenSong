using UnityEngine;
using UnityEngine.Events;
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
    public bool canRecall;
    public bool DontJump { private get; set; }
    public bool DontCrossKick { private get; set; }

    public UnityEvent onRecall = new UnityEvent();

    public static InputReader instance;
    private StunDetection _stunDetection;
    public Vector3 manetteDirection;

    private void Awake()
    {
        //get tous les components
        instance = this;
        _stunDetection = GetComponentInChildren<StunDetection>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.started || context.ReadValue<Vector2>().x == direction.x) return;
        direction = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (DontJump) return;
        if (context.started)
        {
            jump = true;
            PlayerController2D._instance.SetVelocity();
        }

        if (context.canceled) jump = false;
    }

    public void OnFire(InputAction.CallbackContext context) => leftClick = context.performed;

    public void OnAim(InputAction.CallbackContext context) => activateAim = context.performed;

    public void OnStun(InputAction.CallbackContext context)
    {
        if (!context.started || DontCrossKick) return;
        _stunDetection.CrossKick(PlayerController2D._instance.CurrentDirection);
    }

    public void OnDown(InputAction.CallbackContext context) => canDown = context.performed;

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started) PlayerController2D._instance.Roll();
    }
    
    public void OnRecall(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onRecall.Invoke();
            canRecall = true;
        }
        else
        {
            canRecall = false;
        }
    }

    public void ManetteDirection(InputAction.CallbackContext context)
    {
        manetteDirection = context.ReadValue<Vector2>();
    }
}