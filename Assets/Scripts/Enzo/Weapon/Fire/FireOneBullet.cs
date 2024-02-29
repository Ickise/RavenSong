using UnityEngine;

public class FireOneBullet : MonoBehaviour
{
    [Header("À set up")] [SerializeField] private GameObject bullet;

    [SerializeField] private Transform rightShootPosition;
    [SerializeField] private Transform leftShootPosition;
    public Transform rightAimPosition;
    public Transform leftAimPosition;
    public GameObject bulletRef { get; private set; }

    [SerializeField] private SoundData shootAudio;

    public int numberOfAmmo = 1;

    private void Start()
    {
        InputReader.instance.onShoot.AddListener(OnClickToShoot);
    }

    private void OnClickToShoot()
    {
        if (numberOfAmmo == 1)
        {
            bool currentDirection = PlayerController2D._instance.CurrentDirectionAim > 0;

            AudioManager.instance.PlaySound(shootAudio);

            bulletRef = Instantiate(bullet.gameObject,
                transform.position = currentDirection ? rightShootPosition.position : leftShootPosition.position,
                Quaternion.identity);
            numberOfAmmo--;
        }
    }
}