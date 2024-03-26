using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [HideInInspector] public Vector2 direction;

    [HideInInspector] public bool jump;
    [HideInInspector] public bool activateAim = false;
    [HideInInspector] public bool canStun;
    [HideInInspector] public bool canDown;
    public bool DontJump { private get; set; }
    public bool DontCrossKick { private get; set; }

    public UnityEvent<InputAction.CallbackContext> onFire = new();

    public static InputReader instance;
    private StunDetection _stunDetection;
    [HideInInspector] public Vector3 manetteDirection;

    private void Awake()
    {
        //get tous les components
        instance = this;
        _stunDetection = GetComponentInChildren<StunDetection>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.started || context.ReadValue<Vector2>() == direction) return;
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

    public void OnFire(InputAction.CallbackContext context)
    {
        onFire.Invoke(context);
    }

    public void OnAim(InputAction.CallbackContext context) => activateAim = context.performed;

    public void OnCrossKick(InputAction.CallbackContext context)
    {
        if (!context.started || DontCrossKick) return;
        StartCoroutine(_stunDetection.CrossKick(PlayerController2D._instance.CurrentDirectionAim));
    }

    public void OnDown(InputAction.CallbackContext context) => canDown = context.performed;

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started) PlayerController2D._instance.Roll();
    }

    public void ManetteDirection(InputAction.CallbackContext context)
    {
        manetteDirection = context.ReadValue<Vector2>().normalized;
    }
}