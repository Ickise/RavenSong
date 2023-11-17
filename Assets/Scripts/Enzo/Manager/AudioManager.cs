using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { private set; get; }

    [Header("À set up")]
    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip audioClipToPlay)
    {
        audioSource.PlayOneShot(audioClipToPlay);
    }
}