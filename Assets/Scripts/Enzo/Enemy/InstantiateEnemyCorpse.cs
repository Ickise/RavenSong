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
        Instantiate(prefabIACorpse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}