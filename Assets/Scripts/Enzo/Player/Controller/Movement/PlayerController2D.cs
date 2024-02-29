using UnityEngine.VFX;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using Unity.Mathematics;

public class PlayerController2D : MonoBehaviour
{
    [Header("Modifie les mouvements")]
    [SerializeField]
    private float accelerationSpeed = 2f;

    [SerializeField] private float slowSpeed = 0.1f;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxSpeedRecall = 2f;
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
    private RecallBullet _recallBullet;
    private PlayerAnimation _playerAnimation;

    [SerializeField] private VisualEffect VFXDustTrail;

    private float hangTimeCounter;

    private bool canjump = true, canRoll = true, isVFXDustTrailPlaying;

    public bool onRoll;

    public int CurrentDirectionAim { get { return _playerAnimation.GetDirection ? 1 : -1; } }

    public int LastDirection { get; set; } = 1;

    private Vector2 playerVelocity;

    public static PlayerController2D _instance;

    public Vector2 PlayerVelocity
    {
        get { return playerVelocity; }
        set { playerVelocity = value; }
    }

    private float velocityWhenJump;

    private void Awake()
    {
        _instance = this;
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerCollider2D = GetComponent<Collider2D>();
        _raycastDetection = GetComponentInChildren<RaycastDetection>();
        _recallBullet = GetComponent<RecallBullet>();
        VFXDustTrail.Stop();
    }

    private void Update()
    {
        //stop la roulade si elle rencontre du vide ou un mur
        if (onRoll)
        {
            if (DOTween.IsTweening("roll") &&
                (!_raycastDetection.IsGrounded || _raycastDetection.RaycastOnRoll(CurrentDirectionAim)))
                DOTween.Kill("roll");

            return;
        }

        CoyoteTime();
    }

    private void FixedUpdate()
    {
        if (onRoll) return;

        SetGravity();
        SetAirControl();
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
        Debug.DrawRay(transform.position, playerVelocity, Color.green, Time.deltaTime);

        if (InputReader.instance.direction.x == 0)
            LastDirection = CurrentDirectionAim;
        else
            LastDirection = (int)InputReader.instance.direction.x;
    }

    //gère les déplacements du player
    private void ModularMovement()
    {
        if (_raycastDetection.IsGrounded)
        {
            if (!InputReader.instance.jump)
                velocityWhenJump = 0f;
            //calcule le vecteur perpendiculaire a la normal (étant le vecteur up du segment) du segment présent sous les pieds du player
            Vector2 slopNormalPerp = Vector2.Perpendicular(_raycastDetection.IsGrounded.normal).normalized;
            slopNormalPerp.x = -Mathf.Abs(slopNormalPerp.x);
            //sert a annuler le momentum quand le joueur se trouve sur une pente car cela pose des problèmes
            //check si le joueur va dans la direction de son input en les multipliant entre eux, car si l'un des 2 est négatif ça sera inférieur a 0, 
            //ensuite regarde si il est sur une pente, si les 2 sont vrai alors il reset sa velocité x
            if (playerVelocity.x * InputReader.instance.direction.x < 0 && slopNormalPerp.y != 0)
                playerVelocity.x = 0;

            playerVelocity.x += _recallBullet.doRecall
                ? -InputReader.instance.direction.x * slopNormalPerp.x * slowSpeed
                : -InputReader.instance.direction.x * slopNormalPerp.x * accelerationSpeed;
            playerVelocity.x = _recallBullet.doRecall
                ? Mathf.Clamp(playerVelocity.x, -maxSpeedRecall, maxSpeedRecall)
                : Mathf.Clamp(playerVelocity.x, -maxSpeed, maxSpeed);
            playerVelocity.y = SetNormalDirectionY(slopNormalPerp);
            if (!isVFXDustTrailPlaying)
            {
                VFXDustTrail.Play();
                isVFXDustTrailPlaying = true;
            }

            if (InputReader.instance.direction.x == 0)
            {
                if (isVFXDustTrailPlaying)
                {
                    VFXDustTrail.Stop();
                    isVFXDustTrailPlaying = false;
                }

                playerVelocity.x = Mathf.Lerp(playerVelocity.x, 0, groundFriction);
                if (hangTimeCounter < hangTime) return;
                _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.idleBall);
                return;
            }

            if (hangTimeCounter < hangTime) return;
            if (_playerAnimation.GetDirection == InputReader.instance.direction.x > 0)
                _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.walkBall);
            else
                _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.walkBackWard);
        }
        else
        {
            if (isVFXDustTrailPlaying)
            {
                VFXDustTrail.Stop();
                isVFXDustTrailPlaying = false;
            }

            if (velocityWhenJump == 0 && !InputReader.instance.jump) return;
            if (_raycastDetection.RaycastJump && playerVelocity.y > 0f)
                playerVelocity.y = 0f;
        }
    }

    public void SetVelocity() => velocityWhenJump = playerVelocity.x;

    private void Jump()
    {
        hangTimeCounter = 0f;
        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.jump);
        playerVelocity.y = Mathf.Sqrt(-2 * maxHeight * Physics2D.gravity.y * gravityFactor);
    }

    private void CoyoteTime()
    {
        if (_raycastDetection.IsGrounded) hangTimeCounter = hangTime;
        else hangTimeCounter -= Time.deltaTime;
    }

    /// <summary>
    /// check les normals sous le player pour changer sa velocity Y en fonction de la slope
    /// </summary>
    /// <param name="slopNormalPerp"></param>
    private float SetNormalDirectionY(Vector2 slopNormalPerp)
    {
        if (!canjump || Mathf.Abs(slopNormalPerp.y) > 0.75f) return playerVelocity.y;
        if (InputReader.instance.direction.x == 0)
            return (slopNormalPerp.y > 0 ? -1 : 1) * slopNormalPerp.y * Mathf.Abs(playerVelocity.x / slopNormalPerp.x);
        return -InputReader.instance.direction.x * slopNormalPerp.y * Mathf.Abs(playerVelocity.x / slopNormalPerp.x);
    }

    private void SetGravity()
    {
        if (_raycastDetection.IsGrounded)
        {
            playerVelocity.y = 0;
        }
        else
        {
            playerVelocity.y += Physics2D.gravity.y * Time.deltaTime * gravityFactor;
        }
    }

    private void SetAirControl()
    {
        if (!_raycastDetection.IsGrounded)
        {
            playerVelocity.x = velocityWhenJump;
            velocityWhenJump += InputReader.instance.direction.x * accelerationAirControlSpeed;
            velocityWhenJump = Mathf.Clamp(velocityWhenJump, -maxAirControlSpeed, maxAirControlSpeed);
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
        if (DOTween.IsTweening("roll") || !_raycastDetection.IsGrounded || !canRoll) return;
        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.dash);
        Vector2 slopNormalPerp = Vector2.Perpendicular(_raycastDetection.IsGrounded.normal).normalized;
        slopNormalPerp.x = -Mathf.Abs(slopNormalPerp.x);
        playerRigidbody2D.DOMove(transform.position + new Vector3(-slopNormalPerp.x * LastDirection, -slopNormalPerp.y) * distanceRoulade, speedRoulade).SetId("roll")
            .SetSpeedBased(true)
            .OnKill(() =>
            {
                if (_raycastDetection.RaycastOnRoll(CurrentDirectionAim))
                {
                    playerRigidbody2D.velocity = Vector2.zero;
                    // playerRigidbody2D.AddForce(new Vector2(CurrentDirection, 1).normalized * forceBonk,
                    //     ForceMode2D.Impulse);
                }

                _playerAnimation.DontAim = false;
                InputReader.instance.DontCrossKick = false;
                onRoll = false;
                playerCollider2D.enabled = true;
                StartCoroutine(RollCoolDown());
            });
        _playerAnimation.DontAim = true;
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