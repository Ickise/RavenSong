using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { private set; get; }

    [Header("À set up")]
    [SerializeField] private AudioSource mainAudioSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip audioClipToPlay)
    {
        //fonction à appeler dans les autres scripts AudioManager.instance.PlaySFX pour ne jouer qu'une seule fois un son
        mainAudioSource.PlayOneShot(audioClipToPlay);
    }
}