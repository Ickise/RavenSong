using System.Collections;
using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
    private OnBulletHit _onBulletHit;

    private Collider2D corpseCollider;

    private Rigidbody2D corpseRigidbody;

    public static bool bulletInCorpse;

    private void Awake()
    {
        GetComponents();

        bulletInCorpse = true;
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnBulletHit(GameObject bullet)
    {
        StartCoroutine(WaitToDestroyComponents());
    }

    private void GetComponents()
    {
        _onBulletHit = GetComponent<OnBulletHit>();

        corpseCollider = GetComponent<Collider2D>();

        corpseRigidbody = GetComponent<Rigidbody2D>();
    }

    private void DestroyComponents()
    {
        Object[] listToDestroy = { corpseCollider, corpseRigidbody, _onBulletHit, this };

        foreach (var elementToDestroy in listToDestroy)
        {
            Destroy(elementToDestroy);
        }
    }

    private void ChangeLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private IEnumerator WaitToDestroyComponents()
    {
        ChangeLayer();
        yield return new WaitForSeconds(0.1f);
        DestroyComponents();
        StopCoroutine(WaitToDestroyComponents());
    }
}