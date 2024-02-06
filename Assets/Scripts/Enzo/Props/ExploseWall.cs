using UnityEngine;

public class ExploseWall : MonoBehaviour
{
    [SerializeField] private OnBulletHit _onBulletHit;

    private Explodable _explodable;

    private void Awake()
    {
        _explodable = GetComponent<Explodable>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(_explodable.explode);
    }

}