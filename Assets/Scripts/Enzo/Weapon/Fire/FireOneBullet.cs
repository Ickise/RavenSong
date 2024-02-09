using UnityEngine;

public class FireOneBullet : MonoBehaviour
{
    [Header("À set up")] [SerializeField] private GameObject bullet;

    [SerializeField] private Transform rightShootPosition;
    [SerializeField] private Transform leftShootPosition;

    public GameObject bulletRef { get; private set; }

    // [SerializeField] private AudioClip shotAudio;

    public int numberOfAmmo = 1;

    private void Update()
    {
        OnShot();
    }

    private void OnShot()
    {
        
        if (InputReader.instance.leftClick && numberOfAmmo == 1)
        {
            //AudioManager.instance.PlaySFX(shotAudio);
            bool currentDirection = PlayerController2D._instance.CurrentDirection > 0;
            bulletRef = Instantiate(bullet.gameObject,
                transform.position = currentDirection ? rightShootPosition.position : leftShootPosition.position,
                Quaternion.identity);
            numberOfAmmo--;
        }
    }
}