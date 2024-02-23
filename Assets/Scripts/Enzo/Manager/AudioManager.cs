using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { private set; get; }

    private AudioSource mainAudioSource;

    private void Awake()
    {
        instance = this;
        mainAudioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(SoundData data)
    {
        //fonction à appeler dans les autres scripts AudioManager.instance.PlaySFX pour ne jouer qu'une seule fois un son
        mainAudioSource.volume = data.Volume;
        mainAudioSource.pitch = data.GetPitch();
        mainAudioSource.outputAudioMixerGroup = data.AudioMixerGroup;
        mainAudioSource.PlayOneShot(data.AudioToPlay);
    }
    
    public void PlayRandomSound(SoundData[] listOfSoundData)
    {
        SoundData randomClip = listOfSoundData[Random.Range(0, listOfSoundData.Length)];

        PlaySFX(randomClip);
    }
}