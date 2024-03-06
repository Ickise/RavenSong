using Spine;
using Spine.Unity;
using UnityEngine;

public class AnimationsSetter : MonoBehaviour
{
    public static AnimationsSetter instance;

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
        if (animation.skeletonAnimation.skeletonDataAsset != animation.animationReferenceAsset.SkeletonDataAsset)
            animation.skeletonAnimation.skeletonDataAsset = animation.animationReferenceAsset.SkeletonDataAsset;
            
        animation.skeletonAnimation.Initialize(animation.overwriteIniTialize);
        TrackEntry entry = animation.skeletonAnimation.state.SetAnimation(animation.trackNum, animation.animationReferenceAsset, animation.loop);
        entry.TimeScale = animation.speed;
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
