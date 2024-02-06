using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

//gère les animations spine du player
//en singleton
//j'utilise 2 animations (droite et gauche), car le bras change de position quand on se retourne, donc j'active qu'un seul des 2 mesh a la fois mais je joue les animations sur les 2 mesh en même temps
//pour utiliser ce système, il suffit simplement d'écrire cette ligne a l'endroit du code voulu :
//AnimationController.instance.SetCharacterState(AnimationController.AnimationState.[NOM DE L'ANIMATION], [LOOP T-IL ?], [LA VITESSE DE L'ANIMATION VOULU]);
public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;
    [SerializeField] private SkeletonAnimation skeletonAnimationDroite, skeletonAnimationGauche;
    private MeshRenderer meshDroite, meshGauche;
    public bool GetDirection => meshDroite.enabled;

    //la liste des animations, pour en rajouter une, en plus de la mettre ici, il faut aussi la mettre dans le Start() quand on set le dictionnaire
    public enum AnimationState { idleNoBall, idleBall, walkBall };
    private AnimationState currentAnimationState;

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
        meshDroite = skeletonAnimationDroite.GetComponent<MeshRenderer>();
        meshGauche = skeletonAnimationGauche.GetComponent<MeshRenderer>();

        //set le dictionnaire (car il n'est pas serializé dans unity)
        stateAnimationRef = new Dictionary<AnimationState, Animations>()
        {{AnimationState.idleNoBall, animations[(int)AnimationState.idleNoBall]},
        {AnimationState.idleBall, animations[(int)AnimationState.idleBall]},
        {AnimationState.walkBall, animations[(int)AnimationState.walkBall]}};

        //lance l'animation par défaut du player
        SetCharacterState(AnimationState.idleBall, true, 1f);
    }

    //permet de flip l'activation des mesh quand le joueur se retourne, est appelé lors des inputs
    public void FlipAnimation(bool direction)
    {
        meshDroite.enabled = direction;
        meshGauche.enabled = !direction;
    }

    //set les animations sur les 2 mesh
    private void SetAnimation(Animations animation, bool loop, float timeScale)
    {
        skeletonAnimationDroite.skeletonDataAsset = animation.skeletonDataAssetDroite;
        skeletonAnimationDroite.state.SetAnimation(0, animation.droite, loop).TimeScale = timeScale;
        skeletonAnimationGauche.skeletonDataAsset = animation.skeletonDataAssetGauche;
        skeletonAnimationGauche.state.SetAnimation(0, animation.gauche, loop).TimeScale = timeScale;
    }

    //la fonction qui est appelé sur lancer une animations
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
