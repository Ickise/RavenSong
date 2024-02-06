using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
    private GameObject bullet;

    private void Awake()
    {
        bullet = FindObjectOfType<BulletCollisionDetection>().gameObject;
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
        else
        {
            Destroy(gameObject);
        }
    }
}