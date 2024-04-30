using UnityEngine.VFX;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movements")][SerializeField] private float accelerationSpeed = 2f;

    [SerializeField] private float slowSpeed = 0.1f;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxSpeedRecall = 2f;
    [SerializeField] private float groundFriction = 0.3f;

    [SerializeField, Tooltip("En degrés")] private float maxAngleSlop = 72f;

    [Header("Aircontrol")]
    [SerializeField]
    private float accelerationAirControlSpeed = 0.1f;

    [SerializeField] private float maxAirControlSpeed = 4f;

    [SerializeField,
     Tooltip("Lorsque la vitesse de chute du joueur dépasse maxFallSpeed, elle se bloque à cette valeur")]
    private float maxFallSpeed = -20f;

    [Header("Jump")][SerializeField] private float gravityFactor = 1f;
    // private float currentGravity;

    [SerializeField, Header("Sound")] private SoundData[] jumpsoundlist;
    [SerializeField] private SoundData rollsound;

    [SerializeField, Range(1f, 5f)] private float maxHeight = 3f;

    [Header("CoyoteTime")]
    [SerializeField, Range(0.1f, 0.5f)]
    private float hangTime = 0.1f;

    [Header("FallSpeed")]
    [SerializeField, Tooltip("Modifie la rapidité pour tomber après un saut")]
    private float fallMultiplier = 2.5f;

    [SerializeField, Tooltip("Modifie la rapidité pour tomber après le saut minimum")]
    private float lowJumpMultiplier = 2f;

    [Header("Rool")][SerializeField] private float rouladeTime = 0.7f;

    [SerializeField] private float speedRoulade = 6f;
    [SerializeField] private float forceBonk = 10f;
    [SerializeField] private float coolDownToRoll = 2f;

    private Rigidbody2D playerRigidbody2D;

    private BoxCollider2D playerCollider2D;

    private RaycastDetection _raycastDetection;
    private RecallBullet _recallBullet;
    private PlayerAnimation _playerAnimation;
    private StunDetection _stunDetection;
    private FootTriggerPlatform _footTriggerPlatform;

    [Header("VFX")][SerializeField] private VisualEffect VFXDustTrail;
    [SerializeField] private GameObject VFXRoulade, VFXJump;

    [SerializeField] private Transform posVFXRoulade;
    [SerializeField] private Transform posVFXJump;

    private float hangTimeCounter;

    private bool canjump = true, canRoll = true, isVFXDustTrailPlaying;

    [HideInInspector] public bool onRoll;

    public int CurrentDirectionAim
    {
        get { return _playerAnimation.GetDirection ? 1 : -1; }
    }

    public int LastDirection { get; set; } = 1;

    private Vector2 playerVelocity;
    private Vector2 rollDirection;

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
        _stunDetection = GetComponentInChildren<StunDetection>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerCollider2D = GetComponent<BoxCollider2D>();
        _raycastDetection = GetComponentInChildren<RaycastDetection>();
        _recallBullet = GetComponent<RecallBullet>();
        _footTriggerPlatform = GetComponentInChildren<FootTriggerPlatform>();
    }

    private void Start()
    {
        VFXDustTrail.Stop();
        transform.position = Respawn.spawnPosition;
    }

    private void Update()
    {
        // Debug.Log(playerVelocity.y);
        //stop la roulade si elle rencontre du vide ou un mur
        if (onRoll)
        {
            Vector2 slopNormalPerp = Vector2.Perpendicular(_raycastDetection.IsGrounded.normal).normalized;
            slopNormalPerp = Mathf.Abs(slopNormalPerp.y) > maxAngleSlop / 90f
                ? Vector2.left
                : new Vector2(-Mathf.Abs(slopNormalPerp.x), slopNormalPerp.y);
            print(LastDirection);
            rollDirection = new Vector3(-slopNormalPerp.x, -slopNormalPerp.y) * LastDirection;
            if (!_raycastDetection.IsGrounded ||
                Mathf.Abs(Vector2.Perpendicular(_raycastDetection.RaycastOnRoll(rollDirection).normal).normalized.y) >
                maxAngleSlop / 90f)
            {
                DOTween.Kill("roll");
                return;
            }

            if (_raycastDetection.RaycastOnRoll(rollDirection))
            {
                slopNormalPerp = Vector2.Perpendicular(_raycastDetection.RaycastOnRoll(rollDirection).normal)
                    .normalized;
                rollDirection = new Vector3(-slopNormalPerp.x, -slopNormalPerp.y) * LastDirection;
            }

            playerRigidbody2D.velocity = rollDirection * speedRoulade;
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
            LastDirection = Mathf.RoundToInt(InputReader.instance.direction.x);
    }

    //gère les déplacements du player
    private void ModularMovement()
    {
        if (_raycastDetection.IsGrounded)
        {
            // if (!InputReader.instance.jump)
            //     velocityWhenJump = 0f;
            //calcule le vecteur perpendiculaire a la normal (étant le vecteur up du segment) du segment présent sous les pieds du player
            Vector2 slopNormalPerp = Vector2.Perpendicular(_raycastDetection.IsGrounded.normal).normalized;
            slopNormalPerp = Mathf.Abs(slopNormalPerp.y) > maxAngleSlop / 90f
                ? Vector2.left
                : new Vector2(-Mathf.Abs(slopNormalPerp.x), slopNormalPerp.y);
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
                if (FireOneBullet.instance.bulletRef == null && !_stunDetection.IsC2DActive)
                    _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.idleBall);
                else if (_stunDetection.IsC2DActive || _recallBullet.doRecall)
                {
                    _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.none, 0);
                    _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.none, 1);
                }
                else
                    _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.idleNoBall);

                return;
            }

            if (hangTimeCounter < hangTime) return;
            if (FireOneBullet.instance.bulletRef != null || _stunDetection.IsC2DActive)
            {
                _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.walkNoBallHaut);
                _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.walkNoBallBas);
            }
            else
            {
                if (!_stunDetection.IsC2DActive)
                {
                    if (_playerAnimation.GetDirection == InputReader.instance.direction.x > 0)
                        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.walkBall);

                    else
                        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.walkBackWard);
                }
            }
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
        if (!InputReader.instance.canDown)
        {
            VFXInstantieur.instance.PlayVFXInWorld(VFXJump, posVFXJump);
            AudioManager.instance.PlayRandomSound(jumpsoundlist);
            hangTimeCounter = 0f;
            if (FireOneBullet.instance.bulletRef == null && !_stunDetection.IsC2DActive)
                _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.jumpBall);
            else
            {
                _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.JumpNoBallHaut);
                _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.jumpNoBallBas);
            }

            playerVelocity.y = Mathf.Sqrt(-2 * maxHeight * Physics2D.gravity.y * gravityFactor);
        }
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
        if (!canjump) return playerVelocity.y;
        if (InputReader.instance.direction.x == 0)
            return (slopNormalPerp.y > 0 ? -1 : 1) * slopNormalPerp.y * Mathf.Abs(playerVelocity.x / slopNormalPerp.x);
        return -InputReader.instance.direction.x * slopNormalPerp.y * Mathf.Abs(playerVelocity.x / slopNormalPerp.x);
    }

    private void SetGravity()
    {
        if (_raycastDetection.IsGrounded)
        {
            playerVelocity.y = 0;
            //  currentGravity = -0.1f;
        }
        else
        {
            playerVelocity.y += Physics2D.gravity.y * Time.fixedDeltaTime * gravityFactor;
            //            currentGravity += Physics2D.gravity.y * Time.fixedDeltaTime * gravityFactor;
            if (playerVelocity.y < maxFallSpeed)
            {
                playerVelocity.y = maxFallSpeed;
            }
        }
        //      playerVelocity.y += currentGravity;
    }

    private void SetAirControl()
    {
        if (!_raycastDetection.IsGrounded)
        {
            playerVelocity.x = velocityWhenJump;
            velocityWhenJump += InputReader.instance.direction.x * accelerationAirControlSpeed;
            velocityWhenJump = Mathf.Clamp(velocityWhenJump, -maxAirControlSpeed, maxAirControlSpeed);
        }
        else
            velocityWhenJump = playerVelocity.x;
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
        playerVelocity.y += Vector2.up.y * (Physics2D.gravity.y * (factor - 1) * Time.fixedDeltaTime);
    }

    public void Roll()
    {
        if (DOTween.IsTweening("roll") || !_raycastDetection.IsGrounded || !canRoll) return;
        _playerAnimation.SetAnimation(PlayerAnimation.AnimationState.dash);
        // Vector2 slopNormalPerp = Vector2.Perpendicular(_raycastDetection.IsGrounded.normal).normalized;
        // slopNormalPerp.x = -Mathf.Abs(slopNormalPerp.x);

        VFXInstantieur.instance.PlayVFXInWorld(VFXRoulade, posVFXRoulade.position,
            new Vector3(posVFXRoulade.localScale.x * LastDirection, posVFXRoulade.localScale.y,
                posVFXRoulade.localScale.z), VFXRoulade.transform.eulerAngles);
        AudioManager.instance.PlaySound(rollsound);
        // rollDirection = new Vector3(-slopNormalPerp.x, -slopNormalPerp.y) * LastDirection;
        // startRollPos = transform.position;
        // finalRollPos = transform.position + new Vector3(-slopNormalPerp.x, -slopNormalPerp.y) * LastDirection * distanceRoulade;
        float rollTime = 0;
        DOTween.To(() => rollTime, x => rollTime = x, 1f, rouladeTime)
            // playerRigidbody2D
            //     .DOMove(
            //         transform.position +
            //         new Vector3(-slopNormalPerp.x, -slopNormalPerp.y) * LastDirection * distanceRoulade, speedRoulade)
            //     .SetSpeedBased(true)
            .SetId("roll")
            .OnKill(() =>
            {
                playerRigidbody2D.velocity = Vector2.zero;
                if (_raycastDetection.RaycastOnRoll(rollDirection))
                {
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