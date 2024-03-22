using UnityEngine;
using UnityEngine.InputSystem;

public class FireOneBullet : MonoBehaviour
{
    [Header("À set up")] [SerializeField] private GameObject bullet;

    [SerializeField] private Transform rightShootPosition;
    [SerializeField] private Transform leftShootPosition;
    public Transform rightAimPosition;
    public Transform leftAimPosition;
    public GameObject bulletRef { get; set; }

    [SerializeField] private SoundData shootAudio;

    public int numberOfAmmo = 1;

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