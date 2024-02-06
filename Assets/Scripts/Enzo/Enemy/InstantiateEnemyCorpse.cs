using Unity.Mathematics;
using UnityEngine;

public class InstantiateEnemyCorpse : MonoBehaviour
{
    [SerializeField] private GameObject prefabIACorpse;

    [SerializeField] private OnBulletHit _onBulletHit;

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnBulletHit()
    {
        Instantiate(prefabIACorpse, transform.position, quaternion.identity);
        Destroy(gameObject);
    }
}