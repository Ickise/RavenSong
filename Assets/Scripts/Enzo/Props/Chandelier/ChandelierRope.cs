using UnityEngine;

public class ChandelierRope : MonoBehaviour
{
    [SerializeField] private float gravitySpeed = 3f;

    [SerializeField] private Rigidbody2D chandelierRigidbody2D;

    [SerializeField] private OnBulletHit _onBulletHit;

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