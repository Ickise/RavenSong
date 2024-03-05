using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class OnBulletHit : MonoBehaviour
{
    public UnityEvent<GameObject> onBulletHit = new UnityEvent<GameObject>();

    [Tooltip("Mettre true si jamais la balle doit passer à travers l'objet et l'activer")]
    public bool canGoThrough;

    [Tooltip("Mettre true si jamais la balle doit tomber au sol lorsqu'elle touche l'objet")]
    public bool bulletFalling;

    [Tooltip("Mettre false si jamais la balle détruit l'objet qu'elle touche, pour ne pas détruire la balle")]
    public bool putBulletInChildren = true;

    //script à mettre obligatoirement sur les objets qui ont une interaction avec la balle, ne pas oublier de mettre le layer BulletCollision également
    public void BulletHitSomething(GameObject bullet)
    {
        onBulletHit.Invoke(bullet);
    }
}