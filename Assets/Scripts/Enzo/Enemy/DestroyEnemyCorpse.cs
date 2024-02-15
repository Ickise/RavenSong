using UnityEngine;

public class DestroyEnemyCorpse : MonoBehaviour
{
    private OnBulletHit _onBulletHit;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnBulletHit(GameObject bullet)
    {
        //lorsque la balle touche l'ennemi, elle détecte s'il y a une balle, elle se retire des enfants du gameObject
        //lorsque nous rappelons la balle cela détruit le cadavre.
        if (bullet != null)
        {
            bullet.transform.parent = null;
        }
        Destroy(gameObject);
    }
}