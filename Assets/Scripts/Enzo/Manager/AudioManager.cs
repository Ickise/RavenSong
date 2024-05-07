using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { private set; get; }
    public static float volumeScale = 1;
    private AudioSource mainAudioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        mainAudioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundData data)
    {
        //fonction à appeler dans les autres scripts AudioManager.instance.PlaySFX pour ne jouer qu'une seule fois un son
        if (data == null)
        {
            Debug.LogWarning("sound data is not referenced");
            return;
        }

        mainAudioSource.PlayOneShot(SetAudioParameters(data).AudioToPlay);
    }
    
    public void StopSound()
    {
        mainAudioSource.Stop();
    }

    public void PlayRandomSound(SoundData[] listOfSoundData)
    {
        SoundData randomClip = listOfSoundData[Random.Range(0, listOfSoundData.Length)];

        PlaySound(randomClip);
    }

    public void PlayMusic(SoundData data)
    {
        if (data == null)
            return;
        if (mainAudioSource.isPlaying && mainAudioSource.clip == data)
            return;

        mainAudioSource.clip = SetAudioParameters(data).AudioToPlay;
        mainAudioSource.Play();
    }

    private SoundData SetAudioParameters(SoundData soundData)
    {
        mainAudioSource.volume = soundData.Volume * volumeScale;
        mainAudioSource.pitch = soundData.GetPitch();
        mainAudioSource.outputAudioMixerGroup = soundData.AudioMixerGroup;
        return soundData;
    }
}
/*public static AudioManager instance { get; private set; }

[SerializeField] private float timeFadeChangeMusic, volume = 0.5f;
private AudioSource audioSource;

void Awake()
{
    if (instance != null && instance != this)
    {
        Destroy(gameObject);
        return;
    }
    instance = this;
    DontDestroyOnLoad(gameObject);
    audioSource = GetComponent<AudioSource>();
}

public void PlaySound(AudioClip audioClip)
{
    if (audioClip == null)
        return;
        
    audioSource.PlayOneShot(audioClip);
}

public void PlayRandomSound(AudioClip[] listOfSoundData)
{
    AudioClip randomClip = listOfSoundData[Random.Range(0, listOfSoundData.Length)];

    PlaySound(randomClip);
}

public void PlayMusic(AudioClip audioClip)
{
    if (audioClip == null)
        return;
    if (audioSource.isPlaying && audioSource.clip == audioClip)
        return;
    audioSource
        .DOFade(0, timeFadeChangeMusic)
        .OnComplete(() =>
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            audioSource.DOFade(volume, timeFadeChangeMusic);
        });
}

public void StopMusic()
{
    audioSource.clip = null;
}
}*/