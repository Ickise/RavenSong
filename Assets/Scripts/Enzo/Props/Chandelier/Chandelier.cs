using UnityEngine;

[RequireComponent(typeof(OnBulletHit))]
public class Chandelier : MonoBehaviour
{
    private OnBulletHit _onBulletHit;

    private Explodable _explodable;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DestroyObject"))
        {
            Destroy(other.gameObject);
        }
        
        if (other.CompareTag("Platforme"))
        {
            Destroy(transform.parent.gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (other.TryGetComponent(out _explodable))
            {
                Debug.Log(_explodable);
                _explodable.explode(gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void OnBulletHit(GameObject bullet)
    {
        Destroy(transform.parent.gameObject);
    }
}