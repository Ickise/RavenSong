using UnityEngine;
using UnityEngine.Events;

public class OnBulletHit : MonoBehaviour
{
    public UnityEvent onBulletHit = new UnityEvent();
    
    //script à mettre obligatoirement sur les objets qui ont une interaction avec la balle, ne pas oublier de mettre le layer BulletCollision également
    public void BulletHitSomething()
    {
        onBulletHit.Invoke();
    }
}