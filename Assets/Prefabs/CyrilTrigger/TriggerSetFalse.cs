using UnityEngine;

public class TriggerSetFalse : MonoBehaviour
{
    [SerializeField] private GameObject objectFalse;
    [SerializeField] private SoundData soundToPlay;
    [SerializeField] private SoundData musicToPlay;
    [SerializeField] private bool _active;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (objectFalse != null)
                objectFalse.SetActive(_active);
            if (soundToPlay != null)
                AudioManager.instance.PlaySound(soundToPlay);
            if (soundToPlay != null)
                AudioManager.instance.PlayMusic(musicToPlay);
        }
    }
}
