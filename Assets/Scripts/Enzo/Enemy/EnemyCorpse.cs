using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyCorpse : MonoBehaviour
{
    private Collider2D corpseCollider;

    private Rigidbody2D corpseRigidbody;

    public static bool bulletInCorpse;

    [SerializeField, Header("Time to destroy components"), Range(0.1f, 0.5f)] private float timeToDestroyComponents = 0.3f;

    private void Awake()
    {
        GetComponents();

        bulletInCorpse = true;
    }

    private void Update()
    {
        StartCoroutine(WaitToDestroyComponents());
    }

    private void GetComponents()
    {
        corpseCollider = GetComponent<Collider2D>();

        corpseRigidbody = GetComponent<Rigidbody2D>();
    }

    private void DestroyComponents()
    {
        Object[] listToDestroy = { corpseCollider, corpseRigidbody, this };

        foreach (var elementToDestroy in listToDestroy)
        {
            Destroy(elementToDestroy);
        }
    }

    private void ChangeLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private IEnumerator WaitToDestroyComponents()
    {
        ChangeLayer();
        yield return new WaitForSeconds(timeToDestroyComponents);
        DestroyComponents();
        StopCoroutine(WaitToDestroyComponents());
    }
}