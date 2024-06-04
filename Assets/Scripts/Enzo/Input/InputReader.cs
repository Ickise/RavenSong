using System;
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
    [HideInInspector] public bool canInteract;

    [HideInInspector] public int lastDirection;

    public bool DontJump { private get; set; }
    public bool DontCrossKick { private get; set; }

    public UnityEvent<InputAction.CallbackContext> onFire = new();
    public UnityEvent<InputAction.CallbackContext> onParametersChange = new();

    public static InputReader instance;
    public AudioSource audioSourceWalk;
    private StunDetection _stunDetection;
    [HideInInspector] public Vector3 manetteDirection;
    [SerializeField, Header("Sound")] private SoundData runSound;
    [SerializeField] private SoundData runSoundEcho;
    [SerializeField] private SoundData runSoundCatha;
    private SoundData currentRunSound;
    private PlayerAnimation _playerAnimation;

    [HideInInspector] public float tabID = 0;

    private void Awake()
    {
        //get tous les components
        instance = this;
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        _stunDetection = GetComponentInChildren<StunDetection>();
        audioSourceWalk = GetComponent<AudioSource>();
        currentRunSound = runSound;
    }

    private void Update()
    {
        if (PauseController.gameIsPaused)
        {
            audioSourceWalk.Pause();
        }
        else
        {
            audioSourceWalk.UnPause();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (PauseController.gameIsPaused)
        {
            direction = Vector2.zero;
            return;
        }

        if (context.started || context.ReadValue<Vector2>() == direction) return;
        direction = context.ReadValue<Vector2>();

        if (context.performed)
        {
            AudioManager.instance.PlayMusicOnSpecifiedAudioSource(currentRunSound, audioSourceWalk);
            lastDirection = Mathf.RoundToInt(direction.x);
        }
        else
            AudioManager.instance.StopSound(audioSourceWalk);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (PauseController.gameIsPaused) return;

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
        if (PauseController.gameIsPaused) return;

        if (_stunDetection.IsC2DActive || DOTween.IsTweening("roll"))
            return;
        onFire.Invoke(context);
        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.none, 0);
    }

    public void ActivateAim(InputAction.CallbackContext context)
    {
        if (PauseController.gameIsPaused) return;

        if (context.started)
        {
            activateAim = !activateAim;
        }
    }

    public void OnCrossKick(InputAction.CallbackContext context)
    {
        if (PauseController.gameIsPaused) return;

        if (!context.started || DontCrossKick) return;
        StartCoroutine(_stunDetection.CrossKick(PlayerController2D._instance.CurrentDirectionAim));
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        if (PauseController.gameIsPaused) return;

        canDown = context.performed;
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (PauseController.gameIsPaused) return;

        if (context.started) PlayerController2D._instance.Roll();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PauseController._instance.PauseUnPause();
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (PauseController.gameIsPaused) return;

        if (context.performed)
        {
            canInteract = true;
        }
        else if (context.canceled)
        {
            canInteract = false;
        }
    }

    public void ManetteDirection(InputAction.CallbackContext context)
    {
        if (PauseController.gameIsPaused) return;
        manetteDirection = context.ReadValue<Vector2>();
    }

    public void OnTabChange(InputAction.CallbackContext context)
    {
        if (PauseController.gameIsPaused && context.performed)
        {
            float value = context.ReadValue<float>();

            tabID += Mathf.RoundToInt(value);
            tabID = Mathf.Clamp(tabID, -1, 1);

            PauseController._instance.TabChanger();
        }
    }

    public void OnChangeValues(InputAction.CallbackContext context)
    {
        if (!PauseController.gameIsPaused) return;
        onParametersChange.Invoke(context);
    }
}