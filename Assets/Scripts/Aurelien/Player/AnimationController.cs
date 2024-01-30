using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;
    [SerializeField] private SkeletonAnimation skeletonAnimationDroite, skeletonAnimationGauche;

    public enum AnimationState { idle, };
    private AnimationState currentAnimationState;
    [System.Serializable]
    public struct Animations
    {
        public string name;
        public AnimationReferenceAsset droite;
        public AnimationReferenceAsset gauche;
    }
    [SerializeField] private List<Animations> animations = new List<Animations>();
    private Dictionary<AnimationState, Animations> stateAnimationRef = new Dictionary<AnimationState, Animations>();

    private void Start()
    {
        instance = this;
        SetCharacterState(AnimationState.idle, true, 1f);
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skeletonAnimationDroite.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public void SetCharacterState(AnimationState animationState, bool loop, float timeScale)
    {
        Animations animations;
        if (stateAnimationRef.TryGetValue(animationState, out animations))
        {
            currentAnimationState = animationState;
            SetAnimation(animations.droite, loop, timeScale);
        }
    }
}
