using System.Security.Cryptography;
using UnityEngine;

public class BulletRecuperation : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private SphereCastDetection _sphereCastDetection; 
    
    [SerializeField] private AudioClip reloadAudio;
    
    private FireOneBullet _fireOneBullet;

    private void Start()
    {
        _fireOneBullet  = FindObjectOfType<FireOneBullet>();
    }

    private void Update()
    {
        GetBullet();
    }

    private void GetBullet()
    {
        if (_sphereCastDetection.hitPlayer)
        {
            AudioManager.instance.PlaySFX(reloadAudio);
            
            _fireOneBullet.numberOfAmmo ++;
            Destroy(_sphereCastDetection.gameObject);
        }
    }
}