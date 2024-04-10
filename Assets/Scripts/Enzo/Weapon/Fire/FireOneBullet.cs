using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FireOneBullet : MonoBehaviour
{
    [SerializeField, Header("BulletReference")]
    private GameObject bullet;

    [SerializeField, Header("BulletSpawnPosition")]
    private Transform rightBulletSpawnPosition;
    [SerializeField] private Transform leftBulletSpawnPosition;

    [SerializeField, Header("AimPosition")]
    private Transform rightAimPosition;
    [SerializeField] private Transform leftAimPosition;

    [SerializeField, Header("PositionToLook")]
    private Transform rightPositionToLook;
    [SerializeField] private Transform leftPositionToLook;

    [SerializeField, Header("ShootData")] private SoundData shootAudio;

    [Header("NumberOfAmmo"), Range(0f, 1f)]
    public int numberOfAmmo = 1;

    public GameObject bulletRef { get; set; }
    [SerializeField] private GameObject VfxGunShoot;

    private bool currentDirection;

    public static FireOneBullet instance;
    private SpineAim _spineAim;
    private PlayerAnimation _playerAnimation;

    public Transform BulletSpawnPosition => currentDirection ? rightBulletSpawnPosition : leftBulletSpawnPosition;
    public Transform PositionToLook => currentDirection ? rightPositionToLook : leftPositionToLook;
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

            bulletRef = Instantiate(bullet.gameObject, BulletSpawnPosition.position, Quaternion.Euler(Vector3.zero));
            numberOfAmmo--;

            //Debug.Break();
        }
    }
}