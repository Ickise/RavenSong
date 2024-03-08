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
    [SerializeField] private GameObject _animationSetter;
    private SpineAim _spineAim;
    [SerializeField] private SkeletonAnimation skeletonAnimationDroite, skeletonAnimationGauche;
    private MeshRenderer meshDroite, meshGauche;
    public bool GetDirection => meshDroite.enabled;
    public bool DontAim { get; set; }

    //la liste des animations, pour en rajouter une, en plus de la mettre ici, il faut aussi la mettre dans le Start() quand on set le dictionnaire
    public enum AnimationState { none, idleBall, walkBall, walkBackWard, jumpBall, dash, crossKickHaut, recallHaut, jumpNoBallBas, JumpNoBallHaut, walkNoBallBas, walkNoBallHaut, idleNoBall };
    private List<AnimationState> currentAnimationStateOnTrack = new List<AnimationState>();
    public List<AnimationState> GetCurrentAnimationOnEachTrack => currentAnimationStateOnTrack;

    //le struct pour set toute les références des animations dans l'editor
    [Serializable]
    public struct AnimationReference
    {
        public AnimationState name;
        public float speed;
        public int trackNum;
        public bool loop;
        public AnimationReferenceAsset animationReferenceAssetDroite;
        public AnimationReferenceAsset animationReferenceAssetGauche;
    }
    [SerializeField] private AnimationReference[] animations;
    private Dictionary<AnimationState, AnimationReference> animationStateRef = new Dictionary<AnimationState, AnimationReference>();

    private void Start()
    {
        if (AnimationsSetter.instance == null)
            Instantiate(_animationSetter);
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
        for (int i = 0; i < 10; i++)
            currentAnimationStateOnTrack.Add(AnimationState.none);
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

    /// <summary>
    /// Set une animation sur le joueur
    /// </summary>
    /// <param name="animationState"></param>
    public void SetAnimation(AnimationState animationState, int clearTrackIndex = -1)
    {
        if (AnimationsSetter.instance == null)
        {
            Debug.LogWarning("mettre le prefab AnimationController dans la scène");
            return;
        }
        if (clearTrackIndex > -1)
        {
            skeletonAnimationDroite.state.SetEmptyAnimation(clearTrackIndex, 0);
            skeletonAnimationGauche.state.SetEmptyAnimation(clearTrackIndex, 0);
            currentAnimationStateOnTrack[clearTrackIndex] = animationState;
        }
        AnimationReference animationRefAsset;
        if (animationStateRef.TryGetValue(animationState, out animationRefAsset))
        {
            bool overwriteIniTialize = false;
            if (skeletonAnimationDroite.skeletonDataAsset != animationRefAsset.animationReferenceAssetDroite.SkeletonDataAsset)
            {
                overwriteIniTialize = true;
                for (int i = 0; i < currentAnimationStateOnTrack.Count; i++)
                    currentAnimationStateOnTrack[i] = AnimationState.none;
            }
            else if (currentAnimationStateOnTrack[animationRefAsset.trackNum] == animationState)
                return;
            currentAnimationStateOnTrack[animationRefAsset.trackNum] = animationState;
            AnimationsSetter.instance.SetState(new AnimationsSetter.AnimationStructConstructor(animationState.ToString(), skeletonAnimationDroite, animationRefAsset.animationReferenceAssetDroite, animationRefAsset.trackNum, animationRefAsset.speed, animationRefAsset.loop, overwriteIniTialize));
            AnimationsSetter.instance.SetState(new AnimationsSetter.AnimationStructConstructor(animationState.ToString(), skeletonAnimationGauche, animationRefAsset.animationReferenceAssetGauche, animationRefAsset.trackNum, animationRefAsset.speed, animationRefAsset.loop, overwriteIniTialize));
            _spineAim.Start();
        }
    }

    /// <summary>
    /// Set une animation sur le joueur et applique une fonction à sa fin
    /// </summary>
    /// <param name="animationState"></param>
    /// <param name="function"></param>
    public void SetAnimation(AnimationState animationState, Spine.AnimationState.TrackEntryDelegate function)
    {
        if (AnimationsSetter.instance == null)
        {
            Debug.LogWarning("mettre le prefab AnimationController dans la scène");
            return;
        }
        AnimationReference animationRefAsset;
        if (animationStateRef.TryGetValue(animationState, out animationRefAsset))
        {
            bool overrideSkeleton = false;
            if (skeletonAnimationDroite != animationRefAsset.animationReferenceAssetDroite.SkeletonDataAsset)
            {
                overrideSkeleton = true;
                for (int i = 0; i < currentAnimationStateOnTrack.Count; i++)
                    currentAnimationStateOnTrack[i] = AnimationState.none;
            }
            else if (currentAnimationStateOnTrack[animationRefAsset.trackNum] == animationState)
                return;
            currentAnimationStateOnTrack[animationRefAsset.trackNum] = animationState;
            AnimationsSetter.instance.SetState(new AnimationsSetter.AnimationStructConstructor(animationState.ToString(), skeletonAnimationDroite, animationRefAsset.animationReferenceAssetDroite, animationRefAsset.trackNum, animationRefAsset.speed, animationRefAsset.loop, overrideSkeleton));
            AnimationsSetter.instance.SetState(new AnimationsSetter.AnimationStructConstructor(animationState.ToString(), skeletonAnimationGauche, animationRefAsset.animationReferenceAssetGauche, animationRefAsset.trackNum, animationRefAsset.speed, animationRefAsset.loop, overrideSkeleton));
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
