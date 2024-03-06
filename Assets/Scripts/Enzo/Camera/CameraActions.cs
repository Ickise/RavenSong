using Cinemachine;
using UnityEngine;

public class CameraActions : MonoBehaviour
{
    private CinemachineVirtualCamera camera;

    private void Awake()
    {
        camera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    private void SmoothCameraSlideMovement()
    {
        if (true) //dans la condition, il faut regarder où se déplace le PJ, s'il va à gauche ou à droite et faire le déplacement de la caméra, voir la vidéo Youtube.
        //s'il regarde plus d'une seconde il faut que la caméra change de place
        {
            //la caméra va à droite 
        }
        else
        {   
            //la caméra va à gauche
        }
    }

    private void CameraCenter()
    {
        // ici il faut que je regarde lorsque le joueur ne bouge plus du tout
        // S'il ne bouge plus, je centre la caméra sur le joueur
        //Je peux lui donner une position Initiale (Vector3) et qu'il doit toujours revenir à cette positon lorsqu'il n'y a plus de mouvement.
        //Selon Notion : Il faut que Corvus se situe verticalement sur la ligne de la moitie basse de la moitié soit 1/4 de l’écran et horizontalement au centre.
    }
}
