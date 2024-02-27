using System;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

//gère les animations spine du player
//en singleton
//j'utilise 2 animations (droite et gauche), car le bras change de position quand on se retourne, donc j'active qu'un seul des 2 mesh a la fois mais je joue les animations sur les 2 mesh en même temps
//pour utiliser ce système, il suffit simplement d'écrire cette ligne a l'endroit du code voulu :
//AnimationController.instance.SetCharacterState(AnimationController.AnimationState.[NOM DE L'ANIMATION], [LOOP T-IL ?], [LA VITESSE DE L'ANIMATION VOULU]);
public class PlayerAnimation : MonoBehaviour
{
    private SpineAim _spineAim;
    [SerializeField] private SkeletonAnimation skeletonAnimationDroite, skeletonAnimationGauche;
    private MeshRenderer meshDroite, meshGauche;
    public bool GetDirection => meshDroite.enabled;
    public bool DontAim { get; set; }

    //la liste des animations, pour en rajouter une, en plus de la mettre ici, il faut aussi la mettre dans le Start() quand on set le dictionnaire
    public enum AnimationState { idleNoBall, idleBall, walkBall, walkBackWard, jump, dash/*, crossKick, recall */};
    private AnimationState currentAnimationState;
    public AnimationState GetCurrentAnimation => currentAnimationState;

    //le struct pour set toute les références des animations dans l'editor
    [Serializable]
    public struct AnimationReference
    {
        public AnimationState name;
        public float speed;
        public int trackNum;
        public bool loop;
        public bool overrideSkeleton;
        public AnimationReferenceAsset animationReferenceAssetDroite;
        public AnimationReferenceAsset animationReferenceAssetGauche;
    }
    [SerializeField] private AnimationReference[] animations;
    private Dictionary<AnimationState, AnimationReference> animationStateRef = new Dictionary<AnimationState, AnimationReference>();

    private void Start()
    {
        //get les références
        _spineAim = GetComponent<SpineAim>();
        meshDroite = skeletonAnimationDroite.GetComponent<MeshRenderer>();
        meshGauche = skeletonAnimationGauche.GetComponent<MeshRenderer>();

        AnimationState[] animationStateRefArray = Enum.GetValues(typeof(AnimationState)).Cast<AnimationState>().ToArray();
        if (animationStateRefArray.Length < animations.Length)
        {
            Debug.LogWarning("un état d'animation n'est pas énuméré sur " + name);
            return;
        }
        for (int i = 0; i < animationStateRefArray.Length; i++)
            for (int y = 0; y < animations.Length; y++)
                if (animations[y].name == animationStateRefArray[i])
                {
                    animationStateRef.Add(animationStateRefArray[i], animations[y]);
                    break;
                }
        currentAnimationState = AnimationState.idleBall;
    }

    /// <summary>
    /// flip l'activation des mesh quand le joueur se retourne, est appelé lors des inputs
    /// </summary>
    /// <param name="direction"></param>
    public void FlipAnimation(bool direction)
    {
        meshDroite.enabled = direction;
        meshGauche.enabled = !direction;
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
            AnimationsSetter.instance.SetState(new AnimationsSetter.AnimationStructConstructor(animationState.ToString(), skeletonAnimationDroite, animationRefAsset.animationReferenceAssetDroite, animationRefAsset.trackNum, animationRefAsset.speed, animationRefAsset.loop, animationRefAsset.overrideSkeleton));
            AnimationsSetter.instance.SetState(new AnimationsSetter.AnimationStructConstructor(animationState.ToString(), skeletonAnimationGauche, animationRefAsset.animationReferenceAssetGauche, animationRefAsset.trackNum, animationRefAsset.speed, animationRefAsset.loop, animationRefAsset.overrideSkeleton));
            _spineAim.Start();
        }
    }

    private void Update()
    {
        if (DontAim) return;
        //permet de flip l'animation en fonction de la ou le joueur vise
        if (SpineAim.manette && InputReader.instance.manetteDirection != Vector3.zero)
            FlipAnimation(transform.position.x + InputReader.instance.manetteDirection.x > transform.position.x);
        else if (!SpineAim.manette)
            FlipAnimation(Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, Camera.main.nearClipPlane)).x > transform.position.x);
    }
}
