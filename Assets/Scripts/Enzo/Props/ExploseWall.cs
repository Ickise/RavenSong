using UnityEngine;

public class ExploseWall : MonoBehaviour
{
    private OnBulletHit _onBulletHit;

    private Explodable _explodable;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
        _explodable = GetComponent<Explodable>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(_explodable.explode);
       // _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnBulletHit(GameObject bullet)
    {
        if (bullet != null)
        {
            bullet.transform.parent = null;
        }
    }
}