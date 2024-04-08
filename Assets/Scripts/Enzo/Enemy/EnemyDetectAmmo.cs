using UnityEngine;

public class EnemyDetectAmmo : MonoBehaviour
{
    private BulletCollisionDetection _bulletCollisionDetection;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            _bulletCollisionDetection = other.GetComponent<BulletCollisionDetection>();

            if (_bulletCollisionDetection.hasToStop)
            {
                other.isTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (_bulletCollisionDetection != null && _bulletCollisionDetection.hasToStop)
            {
                other.isTrigger = false;
            }
        }
    }
}