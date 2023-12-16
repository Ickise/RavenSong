using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class PlayerController2D : MonoBehaviour
{
    [Header("Modifie les mouvements")]
    [SerializeField] private float accelerationSpeed = 0.1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float groundFriction = 0.3f;

    [Header("Modifie le aircontrol")]
    [SerializeField] private float accelerationAirControlSpeed = 0.1f;
    [SerializeField] private float maxAirControlSpeed = 4f;

    [Header("Modifie le saut")]
    [SerializeField] private float gravityFactor = 1f;
    [SerializeField] private float maxHeight = 3f;

    [Header("Modifie le temps où le joueur saute après avoir quitté une plateforme")]
    [SerializeField] private float hangTime = 0.1f;

    [Header("Modifie la rapidité pour tomber du saut")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Modifie les paramètres de la roulade")]
    // [SerializeField] private float coolDownToRoll = 2f;
    // //[SerializeField] private float speedRoll = 10f;
    // [SerializeField] private float rollDistance = 2f;
    // [SerializeField] private float timeToEnableCollider = 2f;
    [SerializeField] private float distanceRoulade = 4f;
    [SerializeField] private float speedRoulade = 15f;
    [SerializeField] private float forceBonk = 10f;

    [Header("Component à set up")]
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    [SerializeField] private Collider2D playerCollider2D;

    [SerializeField] private RaycastDetection _raycastDetection;

    [Header("Ne pas set up")]
    [SerializeField] private float timeToGetRoll;
    [SerializeField] private float hangTimeCounter;

    private bool onRoll;
    private bool canjump = true;
    public float LastDirection { get; set; } = 1f;

    private Vector2 playerVelocity;
    private float velocityWhenJump;

    private void Update()
    {
        if (onRoll)
        {
            if (DOTween.IsTweening("roll") && (!_raycastDetection.isGrounded || _raycastDetection.RaycastOnRoll(LastDirection)))
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
        ModularMovement();
        SetGravity();
        ComputeGravity();

        if (canjump && InputReader.instance.jump && hangTimeCounter >= 0)
        {
            canjump = false;
            if ((Escalier.isOnEscalier || Plateforme.isOnPlateforme) && InputReader.instance.direction.y == -1) { }
            else Jump();
        }
        else if (!InputReader.instance.jump)
        {
            canjump = true;
        }
        playerRigidbody2D.velocity = playerVelocity;
    }


    private void ModularMovement()
    {
        if (_raycastDetection.isGrounded)
        {
            playerVelocity.x += InputReader.instance.direction.x * accelerationSpeed;
            playerVelocity.x = Mathf.Clamp(playerVelocity.x, -maxSpeed, maxSpeed);
            if (InputReader.instance.direction.x == 0)
                playerVelocity.x = Mathf.Lerp(playerVelocity.x, 0, groundFriction);
        }
        else
        {
            playerVelocity.x = velocityWhenJump;
            velocityWhenJump += InputReader.instance.direction.x * accelerationAirControlSpeed;
            velocityWhenJump = Mathf.Clamp(velocityWhenJump, -maxAirControlSpeed, maxAirControlSpeed);
        }
    }

    public void SetVelocity() => velocityWhenJump = playerVelocity.x;

    private void Jump()
    {
        hangTimeCounter = 0f;

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

    // private void CanRoll()
    // {
    //     timeToGetRoll += Time.deltaTime;

    //     if (timeToGetRoll >= coolDownToRoll && InputReader.instance.canRoll)
    //     {
    //         StartCoroutine(RollInvincibility());

    //         transform.position += InputReader.instance.direction.x > 0 ? Vector3.right * rollDistance : Vector3.left * rollDistance;
    //         StopCoroutine(RollInvincibility());
    //         timeToGetRoll = 0;
    //     }
    // }

    public void Roll()
    {
        if (DOTween.IsTweening(transform) || !_raycastDetection.isGrounded) return;
        transform.localScale += Vector3.down * 0.5f;
        transform.position += Vector3.down * 0.5f;
        playerRigidbody2D.DOMoveX(transform.position.x + distanceRoulade * LastDirection, speedRoulade).SetId("roll").SetSpeedBased(true)
        .OnComplete(() => transform.DOScaleY(1f, 0.2f))
        .OnKill(() =>
        {
            if (_raycastDetection.RaycastOnRoll(LastDirection))
            {
                playerRigidbody2D.velocity = Vector2.zero;
                playerRigidbody2D.AddForce(new Vector2(LastDirection, 1).normalized * forceBonk, ForceMode2D.Impulse);
            }
            transform.DOMoveY(transform.position.y + 0.5f, 0.1f);
            transform.DOScaleY(1f, 0.1f);
        });
        StartCoroutine(RollInvincibility());
    }

    IEnumerator RollInvincibility()
    {
        playerCollider2D.enabled = false;
        onRoll = true;
        yield return new WaitUntil(() => !DOTween.IsTweening("roll"));
        onRoll = false;
        playerCollider2D.enabled = true;
    }
}