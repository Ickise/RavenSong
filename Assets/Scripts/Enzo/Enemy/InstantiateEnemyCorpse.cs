using UnityEngine;

[RequireComponent(typeof(OnBulletHit))]
public class InstantiateEnemyCorpse : MonoBehaviour
{
    [SerializeField, Header("IaCorpsePrefab")]
    private GameObject iaCorpsePrefab;
    [SerializeField] private GameObject VFXDeath;

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
        VFXInstantieur.instance.PlayVFXInWorld(VFXDeath, transform, 3);
        Instantiate(iaCorpsePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}