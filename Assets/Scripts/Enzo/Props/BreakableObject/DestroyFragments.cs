using System;
using UnityEngine;

public class DestroyFragments : MonoBehaviour
{
    
    private OnBulletHit _onBulletHit;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
        _onBulletHit.canGoThrough = true;
    }

    void OnBecameInvisible()
    {
        //lorsque la caméra ne voit plus les objets, cette fonction les détruient
        Destroy(gameObject);
    }
}
