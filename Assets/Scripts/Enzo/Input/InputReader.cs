using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [HideInInspector] public Vector2 direction;

    [HideInInspector] public bool jump;
    [HideInInspector] public bool canJump;
    [HideInInspector] public bool activateAim = false;
    [HideInInspector] public bool canStun;
    [HideInInspector] public bool canDown;
    public int lastDirection;

    public bool DontJump { private get; set; }
    public bool DontCrossKick { private get; set; }

    public UnityEvent<InputAction.CallbackContext> onFire = new();

    public static InputReader instance;
    private StunDetection _stunDetection;
    [HideInInspector] public Vector3 manetteDirection;
    private PlayerAnimation _playerAnimation;

    private void Awake()
    {
        //get tous les components
        instance = this;
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        _stunDetection = GetComponentInChildren<StunDetection>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.started || context.ReadValue<Vector2>() == direction) return;
        direction = context.ReadValue<Vector2>();

        if (context.performed)
        {
            lastDirection = Mathf.RoundToInt(direction.x);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        canJump = context.performed;
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
        if (_stunDetection.IsC2DActive || DOTween.IsTweening("roll"))
            return;
        onFire.Invoke(context);
        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.none, 0);
    }

    public void ActivateAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            activateAim = !activateAim;
        }
    }

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
        manetteDirection = context.ReadValue<Vector2>();
    }
}