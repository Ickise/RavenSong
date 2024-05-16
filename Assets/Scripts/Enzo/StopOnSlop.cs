using UnityEngine;

public class StopOnSlop : MonoBehaviour
{
    private Rigidbody2D bulletRigidbody;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            bulletRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            
            bulletRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
}