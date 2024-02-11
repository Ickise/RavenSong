using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

//modifie la position du bone "aim" sur les deux meshs (voir animationController) pour créer une animation de visée
public class SpineAim : MonoBehaviour
{
    [SerializeField] private string boneAimName;
    private List<Bone> boneAim = new List<Bone>();
    private SkeletonAnimation[] skeletonAnimation;
    private Camera cam;

    //get toute les références
    private void Awake()
    {
        cam = Camera.main;
        skeletonAnimation = GetComponentsInChildren<SkeletonAnimation>();
    }

    public void Start()
    {
        // boneAim = new List<Bone>();
        // for (int i = 0; i < skeletonAnimation.Length; i++)
        //     boneAim.Add(skeletonAnimation[i].skeleton.FindBone(boneAimName));
    }

    private void Update()
    {
        // if (AnimationController.instance.DontAim || boneAim[0] == null) return;
        // //obligé de set individuellement les bone car le bone aim du coté gauche a le x inversé pour des raisons obscure
        // Vector3 localPosDroite = skeletonAnimation[0].transform.InverseTransformPoint(cam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, cam.nearClipPlane)));
        // boneAim[0].SetLocalPosition(new Vector3(Mathf.Clamp(localPosDroite.x, 0, Mathf.Infinity), localPosDroite.y, localPosDroite.z));
        // Vector3 localPosGauche = skeletonAnimation[1].transform.InverseTransformPoint(cam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, cam.nearClipPlane)));
        // boneAim[1].SetLocalPosition(new Vector3(-Mathf.Clamp(localPosGauche.x, Mathf.NegativeInfinity, 0), localPosGauche.y, localPosGauche.z));
    }
}
