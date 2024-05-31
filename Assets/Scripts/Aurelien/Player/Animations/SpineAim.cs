using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using DG.Tweening;

//modifie la position du bone "aim" sur les deux meshs (voir animationController) pour créer une animation de visée
public class SpineAim : MonoBehaviour
{
    private PlayerAnimation _playerAnimation;
    private string boneAimName = "Aim Target - CI";
    private List<Bone> boneAim = new List<Bone>();
    private SkeletonAnimation[] skeletonAnimation;
    private Camera cam;
    [SerializeField] private bool isManette, enableAimLine = true;
    public GameObject aimLineDroite, aimLineGauche, spriteAimDroite, spriteAimGauche, pointerDroite, pointerGauche;
    private Vector3 localPosDroite, localPosGauche;
    private bool animeSpriteAim;
    public static bool manette;

    //get toute les références
    private void Awake()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        cam = Camera.main;
        skeletonAnimation = GetComponentsInChildren<SkeletonAnimation>();
    }

    public void Start()
    {
        manette = isManette;
        boneAim = new List<Bone>();
        for (int i = 0; i < skeletonAnimation.Length; i++)
            boneAim.Add(skeletonAnimation[i].skeleton.FindBone(boneAimName));
    }

    private void Update()
    {
        if (PauseController.gameIsPaused) return;
        // print(_playerAnimation.DontAim.ToString() + "  et  " + (boneAim[0] == null).ToString());
        if (_playerAnimation.DontAim || boneAim[0] == null)
        {
            if (aimLineDroite.activeInHierarchy || aimLineGauche.activeInHierarchy)
            {
                animeSpriteAim = false;

                aimLineDroite.SetActive(false);
                spriteAimDroite.transform.localPosition = Vector3.zero;
                spriteAimDroite.transform.localScale = Vector3.one * 0.1f;

                aimLineGauche.SetActive(false);
                spriteAimGauche.transform.localPosition = Vector3.zero;
                spriteAimGauche.transform.localScale = Vector3.one * 0.1f;
            }
            return;
        }
        if (enableAimLine)
        {
            aimLineDroite.SetActive(_playerAnimation.GetDirection);
            aimLineGauche.SetActive(!_playerAnimation.GetDirection);

            pointerDroite.transform.position = spriteAimDroite.transform.GetChild(0).transform.position;
            pointerDroite.transform.rotation = spriteAimDroite.transform.GetChild(0).transform.rotation;
            
            pointerGauche.transform.position = spriteAimGauche.transform.GetChild(0).transform.position;
            pointerGauche.transform.rotation = spriteAimGauche.transform.GetChild(0).transform.rotation;

            if (!animeSpriteAim)
            {
                animeSpriteAim = true;

                spriteAimDroite.transform.DOScaleX(0.45f, 0.4f).SetEase(Ease.OutQuint);
                spriteAimDroite.transform.DOBlendableLocalMoveBy(Vector2.right * 5, 0.4f);

                spriteAimGauche.transform.DOScaleX(0.45f, 0.4f).SetEase(Ease.OutQuint);
                spriteAimGauche.transform.DOBlendableLocalMoveBy(Vector2.right * 5, 0.4f);
            }
        }

        //obligé de set individuellement les bone car le bone aim du coté gauche a le x inversé pour des raisons obscure
        if (manette && InputReader.instance.manetteDirection != Vector3.zero)
            localPosDroite = localPosGauche = (InputReader.instance.manetteDirection + Vector3.up * 0.25f) * 5f;

        else if (!manette)
        {
            localPosDroite = skeletonAnimation[0].transform.InverseTransformPoint(cam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -cam.transform.position.z)));
            localPosGauche = skeletonAnimation[1].transform.InverseTransformPoint(cam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -cam.transform.position.z)));
        }
        // Vector2 currentArmePos = skeletonAnimation[_playerAnimation.GetDirection ? 0 : 1].skeleton.FindBone("Arme").GetWorldPosition(skeletonAnimation[0].transform);
        // aimLine.transform.position = currentArmePos;
        // aimLine.transform.rotation = Aurinaxtailer.Rotation2D.LookToDirection2D(aimLine.transform.rotation, _playerAnimation.GetDirection ? localPosDroite : localPosGauche);
        boneAim[0].SetLocalPosition(new Vector3(Mathf.Clamp(localPosDroite.x, 1, Mathf.Infinity), localPosDroite.y, localPosDroite.z));
        boneAim[1].SetLocalPosition(new Vector3(-Mathf.Clamp(localPosGauche.x, Mathf.NegativeInfinity, -1), localPosGauche.y, localPosGauche.z));
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(transform.position + (InputReader.instance.manetteDirection + Vector3.up * 0.25f) * 2f, 0.2f);
    // }
}
