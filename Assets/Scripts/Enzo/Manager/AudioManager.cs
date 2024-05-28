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

    public void PlaySound(SoundData data, float pitch = 1)
    {
        if (PauseController.gameIsPaused) return;
        //fonction à appeler dans les autres scripts AudioManager.instance.PlaySFX pour ne jouer qu'une seule fois un son
        if (data == null)
        {
            Debug.LogWarning("sound data is not referenced");
            return;
        }

        mainAudioSource.PlayOneShot(SetAudioParameters(mainAudioSource, data, pitch).AudioToPlay);
    }

    public void StopSound(AudioSource specifiedAudioSource = null)
    {
        if (specifiedAudioSource == null)
            mainAudioSource.Stop();
        else specifiedAudioSource.Stop();
    }

    public void PlayRandomSound(SoundData[] listOfSoundData)
    {
        if (PauseController.gameIsPaused) return;

        SoundData randomClip = listOfSoundData[Random.Range(0, listOfSoundData.Length)];

        if (randomClip == null)
        {
            Debug.LogWarning("sound data is not referenced");
            return;
        }

        PlaySound(randomClip);
    }

    public void PlayMusic(SoundData data)
    {
        if (PauseController.gameIsPaused) return;

        if (data == null)
            return;
        if (mainAudioSource.isPlaying && mainAudioSource.clip == data)
            return;

        mainAudioSource.clip = SetAudioParameters(mainAudioSource, data).AudioToPlay;
        mainAudioSource.Play();
    }

    public void PlayMusicOnSpecifiedAudioSource(SoundData data, AudioSource audioSource)
    {
        if (PauseController.gameIsPaused) return;

        if (data == null)
            return;
        if (audioSource.isPlaying && audioSource.clip == data)
            return;

        audioSource.clip = SetAudioParameters(audioSource, data).AudioToPlay;
        audioSource.Play();
    }

    private SoundData SetAudioParameters(AudioSource audioSource, SoundData soundData, float pitch = 1)
    {
        audioSource.volume = soundData.Volume * volumeScale;
        audioSource.pitch = soundData.GetRandomPitch() * pitch;
        audioSource.outputAudioMixerGroup = soundData.AudioMixerGroup;
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