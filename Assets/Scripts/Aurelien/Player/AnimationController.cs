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
public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;
    private SpineAim _spineAim;
    [SerializeField] private SkeletonAnimation skeletonAnimationDroite, skeletonAnimationGauche;
    public float speedIdleNoBall = 1f, speedIdleBall = 1f, speedWalkBall = 1f, speedJump = 1f, speedWalkBackWard = 1f, speedDash = 1f, speedCrossKick = 1f;
    private MeshRenderer meshDroite, meshGauche;
    public bool GetDirection => meshDroite.enabled;
    public bool DontAim { get; set; }

    //la liste des animations, pour en rajouter une, en plus de la mettre ici, il faut aussi la mettre dans le Start() quand on set le dictionnaire
    public enum AnimationState { idleNoBall, idleBall, walkBall, jump, walkBackWard, dash, crossKick };
    private AnimationState currentAnimationState;
    public AnimationState GetCurrentAnimation => currentAnimationState;

    //le struct pour set toute les références des animations dans l'editor
    [System.Serializable]
    public struct Animations
    {
        public string name;
        public AnimationReferenceAsset droite;
        public SkeletonDataAsset skeletonDataAssetDroite;
        public AnimationReferenceAsset gauche;
        public SkeletonDataAsset skeletonDataAssetGauche;
    }
    [SerializeField] private Animations[] animations;
    private Dictionary<AnimationState, Animations> stateAnimationRef;

    private void Start()
    {
        //get les références
        instance = this;
        _spineAim = GetComponent<SpineAim>();
        meshDroite = skeletonAnimationDroite.GetComponent<MeshRenderer>();
        meshGauche = skeletonAnimationGauche.GetComponent<MeshRenderer>();

        //set le dictionnaire (car il n'est pas serializé dans unity)
        stateAnimationRef = new Dictionary<AnimationState, Animations>()
        {{AnimationState.idleNoBall, animations[(int)AnimationState.idleNoBall]},
        {AnimationState.idleBall, animations[(int)AnimationState.idleBall]},
        {AnimationState.walkBall, animations[(int)AnimationState.walkBall]},
        {AnimationState.jump, animations[(int)AnimationState.jump]},
        {AnimationState.walkBackWard, animations[(int)AnimationState.walkBackWard]},
        {AnimationState.dash, animations[(int)AnimationState.dash]},
        {AnimationState.crossKick, animations[(int)AnimationState.crossKick]}};

        //lance l'animation par défaut du player
        SetCharacterState(0, AnimationState.idleNoBall, true, speedIdleBall, true);
    }

    //permet de flip l'activation des mesh quand le joueur se retourne, est appelé lors des inputs
    public void FlipAnimation(bool direction)
    {
        meshDroite.enabled = direction;
        meshGauche.enabled = !direction;
    }

    //set les animations sur les 2 mesh
    private void SetAnimation(int trackNum, Animations animation, bool loop, float timeScale, bool overwriteIniTialize)
    {
        if (skeletonAnimationDroite.skeletonDataAsset != animation.skeletonDataAssetDroite)
        {
            skeletonAnimationDroite.skeletonDataAsset = animation.skeletonDataAssetDroite;
        }

        skeletonAnimationDroite.Initialize(overwriteIniTialize);
        skeletonAnimationDroite.state.SetAnimation(trackNum, animation.droite, loop).TimeScale = timeScale;

        skeletonAnimationGauche.skeletonDataAsset = animation.skeletonDataAssetGauche;
        skeletonAnimationGauche.Initialize(overwriteIniTialize);
        skeletonAnimationGauche.state.SetAnimation(trackNum, animation.gauche, loop).TimeScale = timeScale;

        _spineAim.Start();
    }

    //la fonction qui est appelé sur lancer une animations
    public void SetCharacterState(int trackEntry, AnimationState animationState, bool loop, float timeScale, bool overwriteIniTialize)
    {
        if (animationState == currentAnimationState) return;
        Animations animations;
        if (stateAnimationRef.TryGetValue(animationState, out animations))
        {
            currentAnimationState = animationState;
            SetAnimation(trackEntry, animations, loop, timeScale, overwriteIniTialize);
        }
    }

    private void Update()
    {
        if (DontAim) return;
        //permet de flip l'animation en fonction de la ou le joueur vise
        FlipAnimation(Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, Camera.main.nearClipPlane)).x > transform.position.x);
    }
}
