using UnityEngine;
using UnityEngine.InputSystem;

public class FireOneBullet : MonoBehaviour
{
    [SerializeField, Header("BulletReference")]
    private GameObject bullet;

    [SerializeField, Header("PlayerShootPositions")]
    private Transform rightShootPosition;

    [SerializeField] private Transform leftShootPosition;

    [SerializeField, Header("ShootData")] private SoundData shootAudio;

    [Header("NumberOfAmmo"), Range(0f, 1f)]
    public int numberOfAmmo = 1;

    public GameObject bulletRef { get; set; }

    private void Start()
    {
        InputReader.instance.onFire.AddListener(OnClickToShoot);
    }

    private void OnClickToShoot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (numberOfAmmo == 1)
        {
            bool currentDirection = PlayerController2D._instance.CurrentDirectionAim > 0;

            AudioManager.instance.PlaySound(shootAudio);

            bulletRef = Instantiate(bullet.gameObject,
                bullet.transform.position = currentDirection ? rightShootPosition.position : leftShootPosition.position,
                Quaternion.identity);
            numberOfAmmo--;

            //Debug.Break();
        }
    }
}