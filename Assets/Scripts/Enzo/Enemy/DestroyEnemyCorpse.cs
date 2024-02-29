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
        _onBulletHit.onBulletHit.AddListener(_onBulletHit.DontDestroyBullet);
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnBulletHit(GameObject bullet)
    {
        Destroy(gameObject);
    }
}