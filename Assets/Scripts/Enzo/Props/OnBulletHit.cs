using UnityEngine;
using UnityEngine.Events;

public class OnBulletHit : MonoBehaviour
{
    public UnityEvent<GameObject> onBulletHit = new UnityEvent<GameObject>();

    public bool canGoThrough;

    [Tooltip("Mettre true si jamais la balle détruit l'objet qu'elle touche, pour ne pas détruire la balle")]
    [SerializeField]
    private bool dontDestroyBullet;

    //script à mettre obligatoirement sur les objets qui ont une interaction avec la balle, ne pas oublier de mettre le layer BulletCollision également
    public void BulletHitSomething(GameObject bullet)
    {
        onBulletHit.Invoke(bullet);
    }

    public void DontDestroyBullet(GameObject bullet)
    {
        if (bullet != null && dontDestroyBullet)
        {
            bullet.transform.parent = null;
        }
    }
}