using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { private set; get; }
    public static float volumeScale = 1;
    [SerializeField] private AudioSource mainAudioSource, secondMainAudioSource, musicAudioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void PlaySound(SoundData data, float pitch = 1)
    {
        //fonction à appeler dans les autres scripts AudioManager.instance.PlaySFX pour ne jouer qu'une seule fois un son
        if (data == null)
        {
            Debug.LogWarning("sound data is not referenced");
            return;
        }
        if (mainAudioSource.isPlaying)
            secondMainAudioSource.PlayOneShot(SetAudioParameters(secondMainAudioSource, data, pitch).AudioToPlay);
        else
            mainAudioSource.PlayOneShot(SetAudioParameters(mainAudioSource, data, pitch).AudioToPlay);
    }

    public void PlaySoundWithSpecificAudioSource(SoundData data, AudioSource specifiedAudioSource = null, float pitch = 1)
    {
        //fonction à appeler dans les autres scripts AudioManager.instance.PlaySFX pour ne jouer qu'une seule fois un son
        if (data == null)
        {
            Debug.LogWarning("sound data is not referenced");
            return;
        }

        specifiedAudioSource.PlayOneShot(SetAudioParameters(specifiedAudioSource, data, pitch).AudioToPlay);
    }

    public void StopSound(AudioSource specifiedAudioSource = null)
    {
        if (specifiedAudioSource == null)
        {
            mainAudioSource.Stop();
            secondMainAudioSource.Stop();
        }
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
        if (musicAudioSource.isPlaying && musicAudioSource.clip == data)
            return;

        musicAudioSource.clip = SetAudioParameters(musicAudioSource, data).AudioToPlay;
        musicAudioSource.Play();
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