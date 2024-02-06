using UnityEngine;
using Spine.Unity;
using Spine;

public class SpineAim : MonoBehaviour
{
    [SerializeField] private string boneAimName;
    private Bone boneAim;
    private SkeletonAnimation skeletonAnimation;
    [SerializeField] private Transform shootPosition;

    private void Start()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        boneAim = skeletonAnimation.skeleton.FindBone(boneAimName);
    }

    private void Update()
    {
        boneAim.SetLocalPosition(shootPosition.parent.InverseTransformPoint(shootPosition.position));
    }
}
