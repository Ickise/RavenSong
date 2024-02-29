using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [Header("Ne pas set up")] public Vector2 direction;

    public bool jump;
    public bool activateAim = false;
    public bool canStun;
    public bool canDown;
    private bool isFirePressed;
    public bool DontJump { private get; set; }
    public bool DontCrossKick { private get; set; }

    public UnityEvent onShoot = new UnityEvent();
    public UnityEvent<bool> onRecall = new();

    public static InputReader instance;
    private StunDetection _stunDetection;
    public Vector3 manetteDirection;

    private PlayerInput _playerInput;
    private InputAction _inputActionFire;

    private void Awake()
    {
        //get tous les components
        instance = this;
        _stunDetection = GetComponentInChildren<StunDetection>();
        _playerInput = GetComponent<PlayerInput>();

        _inputActionFire = _playerInput.actions.FindAction("Shoot");
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

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onShoot.Invoke();
        }
    }

    public void OnAim(InputAction.CallbackContext context) => activateAim = context.performed;

    public void OnStun(InputAction.CallbackContext context)
    {
        if (!context.started || DontCrossKick) return;
        _stunDetection.CrossKick(PlayerController2D._instance.CurrentDirectionAim);
    }

    public void OnDown(InputAction.CallbackContext context) => canDown = context.performed;

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started) PlayerController2D._instance.Roll();
    }

    public void OnRecall(InputAction.CallbackContext context)
    {
        //if (isFirePressed)

        onRecall.Invoke(context.action.IsPressed());
    }

    public void ManetteDirection(InputAction.CallbackContext context)
    {
        manetteDirection = context.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        //  isFirePressed = _inputActionFire.ReadValue<bool>();
    }
}