using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.VFX;
using Spine;
using System.Collections;

public abstract class IA : MonoBehaviour
{
    protected Rigidbody2D rb2D;
    protected Transform player;
    protected LayerMask layerDefault, layerDetectPlayer;
    [SerializeField, Tooltip("direction au start")] protected bool direction; //left = false, right = true
    public bool GetDirection => direction;
    [Header("les statistiques du mob")]
    [SerializeField] protected float speedBalader = 2f, speedAttaquePlayer = 3f;
    [SerializeField, Tooltip("la distance horizontal ou le mob peut voir le joueur")]
    protected float distancePlayerDetection = 10f;
    [SerializeField, Tooltip("la hauteur ou le mob peut voir le joueur")]
    protected float hauteurPlayerDetection = 2f;
    [SerializeField, Tooltip("la distance ou le mob peut attaquer le joueur en melee (le chargeur a aussi une hitbox physique)")]
    protected float distanceAttaqueMelee = 1f;
    [SerializeField, Tooltip("le temps ou le mob va rester en stun time")]
    protected float stunTime = 3f;
    [SerializeField, Tooltip("En degrés")]
    private float maxAngleSlop = 72f;
    protected float jumpForce = 10f;
    protected Vector2 tailleMob;
    [Header("les gizmos")]
    [SerializeField] private bool drawCirclesDetectioninEditor, groundGizmos;
    protected bool overwriteIniTialize = false;
    private int nombreVie = 1;
    public VisualEffect VFXStun;
    protected BoxCollider2D cd2D;
    public int NbVie { get { return nombreVie; } set { nombreVie = value; } }
    protected float currentSpeedMovement;
    protected bool canJump = true;
    protected RaycastHit2D RaycastDetectNotVoid
    {
        get
        {
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * tailleMob.x, Vector2.down * tailleMob.y);
            return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + (direction ? Vector2.right : Vector2.left) * tailleMob.x, Vector2.down, tailleMob.y * 1.5f, layerDefault);
        }
    }
    protected RaycastHit2D IsGrounded { get { return Physics2D.BoxCast(transform.position + new Vector3(cd2D.offset.x + cd2D.edgeRadius, cd2D.offset.y - cd2D.edgeRadius, 0) + Vector3.down * ((cd2D.size.y + cd2D.edgeRadius * 2) / 2f), new Vector2(cd2D.size.x + cd2D.edgeRadius * 2, 0.1f), transform.eulerAngles.z, Vector2.down, 0, layerDefault); } }
    protected RaycastHit2D RaycastHitWall { get { return Physics2D.CapsuleCast(transform.position, new Vector2(0.1f, tailleMob.y * 0.4f), CapsuleDirection2D.Vertical, 0, direction ? Vector2.right : Vector2.left, tailleMob.x, layerDefault); } }
    protected bool DetectPlayer { get { return Mathf.Abs(transform.position.y - player.position.y) < hauteurPlayerDetection && RaycastDetectPlayer && RaycastDetectPlayer.transform.CompareTag("Player"); } }
    private RaycastHit2D RaycastDetectPlayer { get { return Physics2D.Raycast(transform.position, player.position - transform.position, distancePlayerDetection, layerDetectPlayer); } }
    public enum AnimationState { none, moveForward, moveBackward, shoot, shootWalk, idle, mort, stun, stunStart, stunEnd, bonk }
    protected SkeletonAnimation skeletonAnimation;
    [Serializable]
    public struct AnimationReference
    {
        public AnimationState name;
        public float speed;
        public int trackNum;
        public bool loop;
        public AnimationReferenceAsset animationReferenceAsset;
    }
    [SerializeField] private AnimationReference[] animations;
    protected Dictionary<AnimationState, AnimationReference> animationStateRef = new Dictionary<AnimationState, AnimationReference>();
    private AnimationState currentAnimationState;

    protected virtual void Start()
    {
        cd2D = GetComponent<BoxCollider2D>();
        tailleMob = cd2D.size;
        tailleMob.x *= 0.7f;
        tailleMob.y += 0.1f;
        currentSpeedMovement = speedBalader;
        layerDefault = LayerMask.GetMask("Default") | LayerMask.GetMask("Ground") | LayerMask.GetMask("IADontCollide") | LayerMask.GetMask("PlayerDontCollide") | LayerMask.GetMask("Escalier");
        layerDetectPlayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Ground") | LayerMask.GetMask("Player") | LayerMask.GetMask("IADontCollide");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        VFXStun.Stop();
        rb2D = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        AnimationState[] animationStateRefArray = Enum.GetValues(typeof(AnimationState)).Cast<AnimationState>().ToArray();
        for (int i = 0; i < animationStateRefArray.Length; i++)
            for (int y = 0; y < animations.Length; y++)
                if (animations[y].name == animationStateRefArray[i])
                {
                    animationStateRef.Add(animationStateRefArray[i], animations[y]);
                    break;
                }
        currentAnimationState = AnimationState.moveBackward;
        StartCoroutine(SetIdle());
        IEnumerator SetIdle()
        {
            yield return 0;
            SetAnimation(AnimationState.idle);
        }
    }

    protected virtual void Update()
    {
        if (!IsGrounded) return;
        StateManager();
        if (!DetectPlayer) return;
        AtkPlayer();
    }

    protected abstract void StateManager();

    protected void AtkPlayer()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, player.position - transform.position, distanceAttaqueMelee, layerDetectPlayer);
        if (hit2D && hit2D.transform.CompareTag("Player"))
            player.GetComponent<Respawn>().RespawnPlayer();
    }

    protected void RunToDirection(AnimationState animationState = AnimationState.moveForward, bool inverseDirection = false)
    {
        SetAnimation(animationState);
        Vector2 slopNormalPerp = Vector2.Perpendicular(IsGrounded.normal).normalized;
        slopNormalPerp = Mathf.Abs(slopNormalPerp.y) > maxAngleSlop / 90f
                ? Vector2.left
                : new Vector2(-Mathf.Abs(slopNormalPerp.x), slopNormalPerp.y);
        rb2D.velocity = new Vector2(-(direction ? currentSpeedMovement : -currentSpeedMovement) * slopNormalPerp.x, -(direction ? currentSpeedMovement : -currentSpeedMovement) * slopNormalPerp.y);
        if (!inverseDirection)
            transform.localScale = direction ? Vector2.one : new Vector2(-1, 1);
        else
            transform.localScale = !direction ? Vector2.one : new Vector2(-1, 1);
    }

    public void SetAnimation(AnimationState animationState)
    {
        if (currentAnimationState == animationState) return;
        if (AnimationsSetter.instance == null)
        {
            Debug.LogWarning("mettre le prefab AnimationController dans la scène");
            return;
        }
        AnimationReference animationRefAsset;
        if (animationStateRef.TryGetValue(animationState, out animationRefAsset))
        {
            currentAnimationState = animationState;
            AnimationsSetter.instance.SetState(new AnimationsSetter.AnimationStructConstructor(animationState.ToString(), skeletonAnimation, animationRefAsset.animationReferenceAsset, animationRefAsset.trackNum, animationRefAsset.speed, animationRefAsset.loop, overwriteIniTialize));
        }
    }

    public void SetAnimation(AnimationState animationState, Spine.AnimationState.TrackEntryDelegate function)
    {
        if (currentAnimationState == animationState) return;
        if (AnimationsSetter.instance == null)
        {
            Debug.LogWarning("mettre le prefab AnimationController dans la scène");
            return;
        }
        AnimationReference animationRefAsset;
        if (animationStateRef.TryGetValue(animationState, out animationRefAsset))
        {
            currentAnimationState = animationState;
            AnimationsSetter.instance.SetState(new AnimationsSetter.AnimationStructConstructor(animationState.ToString(), skeletonAnimation, animationRefAsset.animationReferenceAsset, animationRefAsset.trackNum, animationRefAsset.speed, animationRefAsset.loop, overwriteIniTialize), function);
        }
    }

    public void Stunning(TrackEntry trackEntry)
    {
        SetAnimation(AnimationState.stun);
        StartCoroutine(TimeStun());
        IEnumerator TimeStun()
        {
            AnimationReference animationRefAsset;
            animationStateRef.TryGetValue(AnimationState.stunEnd, out animationRefAsset);
            yield return new WaitForSeconds(stunTime - animationRefAsset.speed);
            SetAnimation(AnimationState.stunEnd);
            yield return new WaitForSeconds(animationRefAsset.speed);
            enabled = true;
            rb2D.constraints = RigidbodyConstraints2D.None;
            rb2D.freezeRotation = true;
            VFXStun.Stop();
        }
    }

    public void ClearAnimations()
    {
        skeletonAnimation.ClearState();
    }

    void OnDrawGizmos()
    {
        if (groundGizmos)
        {
            if (cd2D == null)
                cd2D = GetComponent<BoxCollider2D>();
            Gizmos.DrawWireCube(transform.position + new Vector3(cd2D.offset.x, cd2D.offset.y, 0) + Vector3.down * ((cd2D.size.y + cd2D.edgeRadius * 2) / 2f), new Vector2(cd2D.size.x + cd2D.edgeRadius * 2, 0.1f));
        }
        if (!drawCirclesDetectioninEditor) return;
        Gizmos.DrawWireSphere(transform.position, distancePlayerDetection);
    }

    private void OnDisable()
    {
        ControlsParameter.GamePadVibration(PlayerController2D._instance, 0.3f, 0.3f, 0.4f);
    }
}
