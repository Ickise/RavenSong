using UnityEngine;

public class ChandelierRope : MonoBehaviour
{
    [Tooltip("Modifie la rapidité de chute du chandelier"), SerializeField, Header("Gravity")]
    private float gravity = 3f;

    [SerializeField, Header("ChandelierRigidbody")]
    private Rigidbody2D chandelierRigidbody2D;

    [SerializeField, Header("Balancing Chandelier")]
    private BalancingChandelier _balancingChandelier;

    private OnBulletHit _onBulletHit;

    [SerializeField, Header("VFX")] private GameObject vfxImpactRope;
    [SerializeField] private SoundData cutRopeSound;

    private void Awake()
    {
        _onBulletHit = GetComponent<OnBulletHit>();
    }

    private void Start()
    {
        _onBulletHit.onBulletHit.AddListener(OnBulletHit);
    }

    public void OnBulletHit(GameObject bullet)
    {
        Destroy(_balancingChandelier);
        chandelierRigidbody2D.isKinematic = false;
        chandelierRigidbody2D.gravityScale = gravity;
        VFXInstantieur.instance.PlayVFXInWorld(vfxImpactRope, transform);
        AudioManager.instance.PlaySound(cutRopeSound);
        Destroy(gameObject);
    }
}