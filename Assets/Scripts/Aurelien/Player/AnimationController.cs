using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;
    [SerializeField] private SkeletonAnimation skeletonAnimationDroite, skeletonAnimationGauche;
    private MeshRenderer meshDroite, meshGauche;
    public bool GetDirection => meshDroite.enabled;

    public enum AnimationState { idle, };
    private AnimationState currentAnimationState;
    [System.Serializable]
    public struct Animations
    {
        public string name;
        public AnimationReferenceAsset droite;
        public AnimationReferenceAsset gauche;
    }
    [SerializeField] private Animations[] animations;
    private Dictionary<AnimationState, Animations> stateAnimationRef;

    private void Start()
    {
        instance = this;
        meshDroite = skeletonAnimationDroite.GetComponent<MeshRenderer>();
        meshGauche = skeletonAnimationGauche.GetComponent<MeshRenderer>();
        stateAnimationRef = new Dictionary<AnimationState, Animations>()
        {{AnimationState.idle, animations[0]}};
        SetCharacterState(AnimationState.idle, true, 1f);
    }

    public void FlipAnimation(bool direction)
    {
        meshDroite.enabled = direction;
        meshGauche.enabled = !direction;
    }

    public void SetAnimation(Animations animation, bool loop, float timeScale)
    {
        skeletonAnimationDroite.state.SetAnimation(0, animation.droite, loop).TimeScale = timeScale;
        skeletonAnimationGauche.state.SetAnimation(0, animation.gauche, loop).TimeScale = timeScale;
    }

    public void SetCharacterState(AnimationState animationState, bool loop, float timeScale)
    {
        Animations animations;
        if (stateAnimationRef.TryGetValue(animationState, out animations))
        {
            currentAnimationState = animationState;
            SetAnimation(animations, loop, timeScale);
        }
    }
}
