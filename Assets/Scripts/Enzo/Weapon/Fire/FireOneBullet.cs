using UnityEngine;

public class FireOneBullet : MonoBehaviour
{
    [Header("À set up")] [SerializeField] private GameObject bullet;

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

            Instantiate(bullet.gameObject, transform.position, Quaternion.identity);
            numberOfAmmo--;
        } 
    }
}