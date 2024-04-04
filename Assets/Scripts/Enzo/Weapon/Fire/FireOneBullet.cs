using UnityEngine;
using UnityEngine.InputSystem;

public class FireOneBullet : MonoBehaviour
{
    [SerializeField, Header("BulletReference")]
    private GameObject bullet;

    [SerializeField, Header("PlayerShootPositions")]
    private Transform rightShootPosition;

    [SerializeField] private Transform leftShootPosition;
    [SerializeField] private Transform rightAimPosition;
    [SerializeField] private Transform leftAimPosition;

    [SerializeField, Header("ShootData")] private SoundData shootAudio;

    [Header("NumberOfAmmo"), Range(0f, 1f)]
    public int numberOfAmmo = 1;

    public GameObject bulletRef { get; set; }
    [SerializeField] private GameObject VfxGunShoot;

    private bool currentDirection;

    public static FireOneBullet instance;
    private SpineAim _spineAim;
    private PlayerAnimation _playerAnimation;

    public Transform ShootPosition => currentDirection ? rightShootPosition : leftShootPosition;
    public Transform AimPosition => currentDirection ? rightAimPosition : leftAimPosition;

    private void Awake()
    {
        instance = this;
        _spineAim = transform.parent.GetComponentInChildren<SpineAim>();
        _playerAnimation = transform.parent.GetComponentInChildren<PlayerAnimation>();
    }

    private void Start()
    {
        InputReader.instance.onFire.AddListener(OnClickToShoot);
    }

    private void OnClickToShoot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (numberOfAmmo == 1)
        {
            currentDirection = PlayerController2D._instance.CurrentDirectionAim > 0;

            AudioManager.instance.PlaySound(shootAudio);
            // VFXInstantieur.instance.PlayVFXInWorld(VfxGunShoot, _playerAnimation.GetDirection ? _spineAim.aimLineDroite.transform.position + _spineAim.aimLineDroite.transform.right  : _spineAim.aimLineGauche.transform.position - _spineAim.aimLineDroite.transform.right , _spineAim.aimLineDroite.transform.localScale, Quaternion.identity);

            bulletRef = Instantiate(bullet.gameObject,
                bullet.transform.position = ShootPosition.position, Quaternion.identity);
            numberOfAmmo--;

            //Debug.Break();
        }
    }
}