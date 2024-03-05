using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "ScriptableObjects/Sound/SoundData", order = 1)]
public class SoundData : ScriptableObject
{
    [SerializeField] private float volume = 1f;
    [SerializeField] private float pitchVariation = 0.2f;

    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [SerializeField] private AudioClip audioToPlay;

    public float Volume => volume;
    public AudioMixerGroup AudioMixerGroup => audioMixerGroup;
    public AudioClip AudioToPlay => audioToPlay;

    public float GetPitch()
    {
        return 1 + Random.Range(-pitchVariation, pitchVariation);
    }
}