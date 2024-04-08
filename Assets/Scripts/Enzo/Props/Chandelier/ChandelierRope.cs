using UnityEngine;

public class ChandelierRope : MonoBehaviour
{
    [Tooltip("Modifie la rapidité de chute du chandelier"), SerializeField, Header("Gravity")]
    private float gravity = 3f;

    [SerializeField, Header("ChandelierRigidbody")] private Rigidbody2D chandelierRigidbody2D;

    private OnBulletHit _onBulletHit;
    [SerializeField] private GameObject vfxImpactRope;

    private Animator parentAnimator;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
        // chandelierRigidbody2D = GetComponentInChildren<Rigidbody2D>();

        parentAnimator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    private void OnBulletHit(GameObject bullet)
    {
        parentAnimator.enabled = false;
        chandelierRigidbody2D.isKinematic = false;
        chandelierRigidbody2D.gravityScale = gravity;
        VFXInstantieur.instance.PlayVFXInWorld(vfxImpactRope, transform);
        Destroy(gameObject);
    }
}