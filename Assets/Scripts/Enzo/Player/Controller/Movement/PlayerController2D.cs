using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerController2D : MonoBehaviour
{
    [Header("Modifie les mouvements")]
    [SerializeField]
    private float accelerationSpeed = 0.1f;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float groundFriction = 0.3f;

    [Header("Modifie le aircontrol")]
    [SerializeField]
    private float accelerationAirControlSpeed = 0.1f;

    [SerializeField] private float maxAirControlSpeed = 4f;

    [Header("Modifie le saut")]
    [SerializeField]
    private float gravityFactor = 1f;

    [SerializeField] private float maxHeight = 3f;

    [Header("Modifie le temps où le joueur peut sauter après avoir quitté une plateforme")]
    [SerializeField]
    private float hangTime = 0.1f;

    [Header("Modifie la rapidité pour tomber après un saut")]
    [SerializeField]
    private float fallMultiplier = 2.5f;

    [Header("Modifie la rapidité pour tomber après le saut minimum")]
    [SerializeField]
    private float lowJumpMultiplier = 2f;

    [Header("Modifie les paramètres de la roulade")]
    [SerializeField]
    private float distanceRoulade = 4f;

    [SerializeField] private float speedRoulade = 15f;
    [SerializeField] private float forceBonk = 10f;
    [SerializeField] private float coolDownToRoll = 2f;

    private Rigidbody2D playerRigidbody2D;

    private Collider2D playerCollider2D;

    private RaycastDetection _raycastDetection;

    private float hangTimeCounter;

    private bool onRoll, canjump = true, canRoll = true;
    public int CurrentDirection { get { return AnimationController.instance.GetDirection ? 1 : -1; } }

    private Vector2 playerVelocity;

    public Vector2 PlayerVelocity
    {
        get { return playerVelocity; }
        set { playerVelocity = value; }
    }

    private float velocityWhenJump;

    private void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerCollider2D = GetComponent<Collider2D>();
        _raycastDetection = GetComponentInChildren<RaycastDetection>();
    }

    private void Update()
    {
        //stop la roulade si elle rencontre du vide ou un mur
        if (onRoll)
        {
            if (DOTween.IsTweening("roll") &&
                (!_raycastDetection.isGrounded || _raycastDetection.RaycastOnRoll(CurrentDirection)))
            {
                onRoll = false;
                playerCollider2D.enabled = true;
                DOTween.Kill("roll");
                StopAllCoroutines();
            }

            return;
        }

        CoyoteTime();
    }

    private void FixedUpdate()
    {
        if (onRoll) return;

        SetGravity();
        ComputeGravity();
        if (canjump && InputReader.instance.jump && hangTimeCounter >= 0)
        {
            canjump = false;
            Jump();
        }
        else if (!InputReader.instance.jump)
            canjump = true;

        ModularMovement();
        playerRigidbody2D.velocity = playerVelocity;
    }

    //gère les déplacements du player
    private void ModularMovement()
    {
        if (_raycastDetection.isGrounded)
        {
            if (!InputReader.instance.jump)
                velocityWhenJump = 0f;
            playerVelocity.x += InputReader.instance.direction.x * accelerationSpeed;
            playerVelocity.x = Mathf.Clamp(playerVelocity.x, -maxSpeed, maxSpeed);
            if (InputReader.instance.direction.x == 0)
            {
                playerVelocity.x = Mathf.Lerp(playerVelocity.x, 0, groundFriction);
                if (!canjump) return;
                AnimationController.instance.SetCharacterState(AnimationController.AnimationState.idleBall, true, AnimationController.instance.speedIdleBall);
                return;
            }
            if (!canjump) return;
            if (AnimationController.instance.GetDirection == InputReader.instance.direction.x > 0)
                AnimationController.instance.SetCharacterState(AnimationController.AnimationState.walkBall, true, AnimationController.instance.speedWalkBall);
            else
                AnimationController.instance.SetCharacterState(AnimationController.AnimationState.walkBackWard, true, AnimationController.instance.speedWalkBackWard);
        }
        else
        {
            if (velocityWhenJump == 0 && !InputReader.instance.jump) return;
            if (_raycastDetection.RaycastJump && playerVelocity.y > 0f)
                playerVelocity.y = 0f;
            playerVelocity.x = velocityWhenJump;
            velocityWhenJump += InputReader.instance.direction.x * accelerationAirControlSpeed;
            velocityWhenJump = Mathf.Clamp(velocityWhenJump, -maxAirControlSpeed, maxAirControlSpeed);
        }
    }

    public void SetVelocity() => velocityWhenJump = playerVelocity.x;

    private void Jump()
    {
        hangTimeCounter = 0f;
        AnimationController.instance.SetCharacterState(AnimationController.AnimationState.jump, false, AnimationController.instance.speedJump);
        playerVelocity.y = Mathf.Sqrt(-2 * maxHeight * Physics2D.gravity.y * gravityFactor);
    }

    private void CoyoteTime()
    {
        if (_raycastDetection.isGrounded) hangTimeCounter = hangTime;
        else hangTimeCounter -= Time.deltaTime;
    }

    private void SetGravity()
    {
        if (_raycastDetection.isGrounded)
        {
            playerVelocity.y = 0;
        }
        else
        {
            playerVelocity.y += Physics2D.gravity.y * Time.deltaTime * gravityFactor;
        }
    }

    private void ComputeGravity()
    {
        bool isFalling = playerVelocity.y < 0;
        bool isReleasingJump = playerVelocity.y > 0 && !InputReader.instance.jump;

        if (!isFalling && !isReleasingJump)
        {
            return;
        }

        var factor = isFalling ? fallMultiplier : lowJumpMultiplier;
        playerVelocity += Vector2.up * (Physics2D.gravity.y * (factor - 1) * Time.deltaTime);
    }

    public void Roll()
    {
        if (DOTween.IsTweening(transform) || !_raycastDetection.isGrounded || !canRoll) return;
        AnimationController.instance.SetCharacterState(AnimationController.AnimationState.dash, false, AnimationController.instance.speedDash);
        playerRigidbody2D.DOMoveX(transform.position.x + distanceRoulade * CurrentDirection, speedRoulade).SetId("roll")
            .SetSpeedBased(true)
            .OnKill(() =>
            {
                if (_raycastDetection.RaycastOnRoll(CurrentDirection))
                {
                    playerRigidbody2D.velocity = Vector2.zero;
                    playerRigidbody2D.AddForce(new Vector2(CurrentDirection, 1).normalized * forceBonk,
                        ForceMode2D.Impulse);
                }
                AnimationController.instance.DontAim = false;
                InputReader.instance.DontCrossKick = false;
                onRoll = false;
                playerCollider2D.enabled = true;
                StartCoroutine(RollCoolDown());
            });
        AnimationController.instance.DontAim = true;
        InputReader.instance.DontCrossKick = true;
        playerCollider2D.enabled = false;
        onRoll = true;
    }

    IEnumerator RollCoolDown()
    {
        canRoll = false;
        yield return new WaitForSeconds(coolDownToRoll);
        canRoll = true;
    }
}