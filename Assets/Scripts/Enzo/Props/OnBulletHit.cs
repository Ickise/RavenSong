using UnityEngine;
using UnityEngine.Events;

public class OnBulletHit : MonoBehaviour
{
    public UnityEvent<GameObject> onBulletHit = new UnityEvent<GameObject>();

    public bool canGoThrough;

    //script à mettre obligatoirement sur les objets qui ont une interaction avec la balle, ne pas oublier de mettre le layer BulletCollision également
    public void BulletHitSomething(GameObject bullet)
    {
        onBulletHit.Invoke(bullet);
    }
}