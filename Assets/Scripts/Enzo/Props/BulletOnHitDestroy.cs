using UnityEngine;

public class BulletOnHitDestroy : MonoBehaviour
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
        Destroy(bullet);
        FireOneBullet.instance.numberOfAmmo = 1;
    }
}