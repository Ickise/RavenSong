using UnityEngine;

public class RecallBullet : MonoBehaviour
{
    private GameObject bullet;

    private Rigidbody2D bulletRigidbody;

    private Vector2 direction;

    private bool isMoving;

    [SerializeField] private FireOneBullet _fireOneBullet;

    [SerializeField] private float destroyDistance = 0.6f;
    [SerializeField] private float delayBeforeMove = 1.5f;
    [SerializeField] private float speedToRecall = 2f;
    [SerializeField] private float speedDelay = 0.5f;
    [SerializeField] private float distanceToRecall = 3f;

    private float distance;

    private void Start()
    {
        InputReader.instance.onRecall.AddListener(OnClick);
    }

    private void Update()
    {
        GetDelayBeforeMove();
      
        if (InputReader.instance.canRecall && transform.position.x < distanceToRecall)
        {
            Invoke("RecallAmmo", delayBeforeMove);
        }
        else
        {
            CancelInvoke("RecallAmmo");
        }
    }

    private void GetDelayBeforeMove()
    {
        if (bullet != null)
        {
            direction = transform.position - bullet.transform.position;
            distance = direction.magnitude;
            
            delayBeforeMove = distance * speedDelay;

            Debug.Log(delayBeforeMove);
        }
    }
    
    public void RecallAmmo()
    {
        if (bullet != null)
        {
            bulletRigidbody.velocity = Vector2.zero;

            bullet.transform.Translate(direction.normalized * (speedToRecall * Time.deltaTime));

            if (distance <= destroyDistance)
            {
                Destroy(bullet);
                _fireOneBullet.numberOfAmmo = 1;
            }
        }
    }

    private void OnClick()
    {
        bullet = FindObjectOfType<BulletCollisionDetection>().gameObject;

        if (bullet != null)
        {
            bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            Destroy(bullet.GetComponent<BulletVelocity>());
        }
    }
}