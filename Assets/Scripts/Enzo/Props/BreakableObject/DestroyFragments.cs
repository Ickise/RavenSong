using System.Collections;
using UnityEngine;

public class DestroyFragments : MonoBehaviour
{
    private OnBulletHit _onBulletHit;

    public bool waitToDestroy;

    public float timeToDestroy;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
        _onBulletHit.canGoThrough = true;
    }

    private void Update()
    {
        DestroyGameObject();
    }

    void OnBecameInvisible()
    {
        //lorsque la caméra ne voit plus les objets, cette fonction les détruient
        if (!waitToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void DestroyGameObject()
    {
        if (waitToDestroy)
        {
            StartCoroutine(WaitSecToDestroy());
        }
    }

    private IEnumerator WaitSecToDestroy()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}