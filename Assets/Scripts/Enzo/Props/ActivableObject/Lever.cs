using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool isActive;

    private GameObject bullet;

    private BulletCollisionDetection _bulletCollisionDetection;

    private OnBulletHit _onBulletHit;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void Update()
    {
        BulletMotionless();
    }

    private void BulletMotionless()
    {
        if (bullet != null && !InputReader.instance.canRecall)
        {
            bullet.transform.position = gameObject.transform.position;
        }
    }

    private void OnBulletHit()
    {
        bullet = FindObjectOfType<BulletCollisionDetection>().gameObject;

        _bulletCollisionDetection = bullet.GetComponent<BulletCollisionDetection>();
        _bulletCollisionDetection.enabled = false;

        CanChangeBool();
    }

    private void CanChangeBool()
    {
        if (!isActive)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }
}