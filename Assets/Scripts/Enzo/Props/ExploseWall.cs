using UnityEngine;
using UnityEngine.Serialization;

public class ExploseWall : MonoBehaviour
{
    private OnBulletHit _onBulletHit;

    private Explodable _explodable;

    [Tooltip("À mettre sur true si vous voulez détruire les fragments après X secondes")] public bool destroyFragmentsAfterSeconds = false;

    public float timeToDestroyFragments;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
        _explodable = GetComponent<Explodable>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(_explodable.explode);
    }
}