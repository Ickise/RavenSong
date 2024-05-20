using UnityEngine;
using UnityEngine.InputSystem;

public class FireOneBullet : MonoBehaviour
{
    [SerializeField, Header("BulletReference")]
    private GameObject bullet;

    [SerializeField, Header("BulletSpawnPosition")]
    private Transform rightBulletSpawnPosition;

    [SerializeField] private Transform leftBulletSpawnPosition;

    [SerializeField, Header("PositionToLook")]
    private Transform rightPositionToLook;

    [SerializeField] private Transform leftPositionToLook;

    [SerializeField, Header("ShootData")] private SoundData[] shootsAudio;

    [Header("NumberOfAmmo"), Range(0f, 1f)]
    public int numberOfAmmo = 1;

    public GameObject bulletRef { get; set; }
    [SerializeField] private GameObject VfxGunShoot;

    private bool currentDirection;

    public static FireOneBullet instance;

    public Transform BulletSpawnPosition => currentDirection ? rightBulletSpawnPosition : leftBulletSpawnPosition;
    public Transform PositionToLook => currentDirection ? rightPositionToLook : leftPositionToLook;

    private void Awake()
    {
        instance = this;
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
            AudioManager.instance.PlayRandomSound(shootsAudio);
            ControlsParameter.GamePadVibration(this, 0.1f, 0.1f, 0.1f);

            bulletRef = Instantiate(bullet.gameObject, BulletSpawnPosition.position, Quaternion.Euler(Vector3.zero));
            numberOfAmmo--;

            //Debug.Break();
        }
    }
}