using UnityEngine;

[RequireComponent(typeof(OnBulletHit))]
public class ExploseWall : MonoBehaviour
{
    private OnBulletHit _onBulletHit;

    private Explodable _explodable;

    [Tooltip("À mettre sur true si vous voulez détruire les fragments après X secondes"), Header("Bool")]
    public bool destroyFragmentsAfterSeconds = false;

    [Header("TimeToDestroyFragments"), Tooltip(
         "Temps en seconde à changer s'il faut détruire les fragments et mettre le temps désiré pour qu'ils se détruisent après ce temps")]
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