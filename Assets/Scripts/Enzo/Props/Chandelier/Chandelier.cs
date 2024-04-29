using UnityEngine;

[RequireComponent(typeof(OnBulletHit))]
public class Chandelier : MonoBehaviour
{
    private Explodable _explodable;
    [SerializeField] private GameObject VfxExplosion;
    
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

    private void OnDestroy()
    {
        VFXInstantieur.instance.PlayVFXInWorld(VfxExplosion, transform);
    }
}