using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

public class AnimationsSetter : MonoBehaviour
{
    public static AnimationsSetter instance;
    public delegate void DelegateFunction(TrackEntry trackEntry);
    public DelegateFunction delegateFunction;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void SetState(AnimationStructConstructor animation)
    {
        if (animation.overwriteIniTialize)
        {
            animation.skeletonAnimation.skeletonDataAsset = animation.animationReferenceAsset.SkeletonDataAsset;
            animation.skeletonAnimation.Initialize(true);
        }

        TrackEntry entry = animation.skeletonAnimation.state.SetAnimation(animation.trackNum, animation.animationReferenceAsset, animation.loop);
        entry.TimeScale = animation.speed;
        // if (!animation.loop)
        //     StartCoroutine(ClearAnimationOnTrack(entry));
    }

    public void SetState(AnimationStructConstructor animation, Spine.AnimationState.TrackEntryDelegate function)
    {
        if (animation.skeletonAnimation.skeletonDataAsset != animation.animationReferenceAsset.SkeletonDataAsset)
        {
            animation.skeletonAnimation.skeletonDataAsset = animation.animationReferenceAsset.SkeletonDataAsset;
            animation.skeletonAnimation.Initialize(animation.overwriteIniTialize);
        }

        TrackEntry entry = animation.skeletonAnimation.state.SetAnimation(animation.trackNum, animation.animationReferenceAsset, animation.loop);
        entry.TimeScale = animation.speed;
        entry.Complete += function;
    }

    private IEnumerator ClearAnimationOnTrack(TrackEntry entry)
    {
        yield return new WaitForSpineAnimation(entry, WaitForSpineAnimation.AnimationEventTypes.Complete);
    }

    public struct AnimationStructConstructor
    {
        public string name;
        public SkeletonAnimation skeletonAnimation;
        public AnimationReferenceAsset animationReferenceAsset;
        public float speed;
        public int trackNum;
        public bool loop;
        public bool overwriteIniTialize;
        public AnimationStructConstructor(string name, SkeletonAnimation skeletonAnimation, AnimationReferenceAsset animationReferenceAsset, int trackNum, float speed, bool loop, bool overwriteIniTialize)
        {
            this.name = name;
            this.skeletonAnimation = skeletonAnimation;
            this.animationReferenceAsset = animationReferenceAsset;
            this.speed = speed;
            this.trackNum = trackNum;
            this.loop = loop;
            this.overwriteIniTialize = overwriteIniTialize;
        }
    }
}
