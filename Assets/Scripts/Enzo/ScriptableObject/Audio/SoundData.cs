using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "ScriptableObjects/Sound/SoundData", order = 1)]
public class SoundData : ScriptableObject
{
    [SerializeField, Header("AudioParameters"), Range(0f,1f)] private float volume = 1f;
    [SerializeField, Range(0f,1f), Tooltip("Mettre à 0 si on ne veut pas de variation et qu'il soit toujours à 1")] private float pitchVariation = 0.2f;

    [SerializeField, Header("AudioMixerGroup")] private AudioMixerGroup audioMixerGroup;
    
    [SerializeField, Header("AudioClipToPlay")] private AudioClip audioToPlay;

    public float Volume => volume;
    public AudioMixerGroup AudioMixerGroup => audioMixerGroup;
    public AudioClip AudioToPlay => audioToPlay;

    public float GetRandomPitch()
    {
        return 1 + Random.Range(-pitchVariation, pitchVariation);
    }
}