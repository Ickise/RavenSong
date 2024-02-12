using UnityEngine;

public class Chandelier : MonoBehaviour
{
    private OnBulletHit _onBulletHit;
    
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

        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnBulletHit(GameObject bullet)
    {
        Destroy(transform.parent.gameObject);
    }
}