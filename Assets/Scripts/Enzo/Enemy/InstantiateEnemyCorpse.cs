using UnityEngine;

public class InstantiateEnemyCorpse : MonoBehaviour
{
    [SerializeField] private GameObject prefabIACorpse;

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
        //lorsque la balle touche l'ennemi, cela fait appraître un cadavre et détruit l'ennemi
        if (bullet != null)
        {
            bullet.transform.parent = null;
        }

        Instantiate(prefabIACorpse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}