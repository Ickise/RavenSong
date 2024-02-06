using UnityEngine;

public class ChandelierRope : MonoBehaviour
{
    [Header("Modifie la rapidité de chute du chandelier")] [SerializeField] private float gravitySpeed = 3f;

    private Rigidbody2D chandelierRigidbody2D;

    private OnBulletHit _onBulletHit;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
        chandelierRigidbody2D = GetComponentInParent<Rigidbody2D>();
    }
    
    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnBulletHit()
    {
        chandelierRigidbody2D.isKinematic = false;
        chandelierRigidbody2D.gravityScale = gravitySpeed;

        Destroy(gameObject);
    }
}