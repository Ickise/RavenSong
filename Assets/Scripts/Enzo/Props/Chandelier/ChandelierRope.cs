using UnityEngine;

public class ChandelierRope : MonoBehaviour
{
    [SerializeField] private GameObject chandelier;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            chandelier.AddComponent<Rigidbody2D>();
            Destroy(gameObject);
        }
    }
}
