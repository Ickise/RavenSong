using UnityEngine;

public class ResetAmmo : MonoBehaviour
{
    private GameObject m_bullet;

    public void ResetBullet()
    {
        m_bullet = FindObjectOfType<BulletCollisionDetection>().gameObject;
        Destroy(m_bullet);
        FireOneBullet.instance.numberOfAmmo = 1;
    }
}