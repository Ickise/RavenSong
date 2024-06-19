using UnityEngine;
using UnityEngine.Events;

public class OnBulletHit : MonoBehaviour
{
    [Header("Unity Events"),
     Tooltip(
         "Mettre dans cet événement les fonctions ou actions que l'objet effectura lorsque la balle entrera en contact avec lui")]
    public UnityEvent<GameObject> onBulletHit = new UnityEvent<GameObject>();

    [Header("Bools")] [Tooltip("Mettre true si la balle doit passer à travers l'objet et l'activer")]
    public bool canGoThrough;

    [Tooltip("Mettre true si la balle doit tomber au sol lorsqu'elle a touché l'objet")]
    public bool bulletFalling;

    //script à mettre obligatoirement sur les objets qui ont une interaction avec la balle, ne pas oublier de mettre le layer BulletCollision également
    public void BulletHitSomething(GameObject bullet)
    {
        onBulletHit.Invoke(bullet);
    }
}