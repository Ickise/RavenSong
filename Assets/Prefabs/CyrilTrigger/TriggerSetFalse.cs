using DG.Tweening;
using UnityEngine;

public class TriggerSetFalse : MonoBehaviour
{
    [SerializeField] private GameObject objectFalse;
    [SerializeField] private SoundData soundToPlay;
    [SerializeField] private SoundData musicToPlayIntro, musicToPlayLoop;
    [SerializeField] private bool _active, destroySelf;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (objectFalse != null)
                objectFalse.SetActive(_active);
            if (soundToPlay != null)
                AudioManager.instance.PlaySound(soundToPlay);
            if (musicToPlayLoop != null)
            {
                AudioManager.instance.PlayMusic(musicToPlayIntro);
                float a = 0;
                DOTween.To(() => a, x => a = x, 1, musicToPlayIntro.AudioToPlay.length)
                .OnComplete(() => AudioManager.instance.PlayMusic(musicToPlayLoop));
            }
            if (destroySelf)
                Destroy(gameObject);
        }
    }
}
