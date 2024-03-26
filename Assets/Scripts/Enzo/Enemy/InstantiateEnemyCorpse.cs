using UnityEngine;

[RequireComponent(typeof(OnBulletHit))]
public class InstantiateEnemyCorpse : MonoBehaviour
{
    [SerializeField, Header("IaCorpsePrefab")]
    private GameObject iaCorpsePrefab;

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
        Instantiate(iaCorpsePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}