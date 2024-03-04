using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] listToDisable;
    [SerializeField] private GameObject[] listToEnable;

    [SerializeField] private VideoPlayer videoPlayer;

    private void Update()
    {
        UnloadVideo();
    }

    public void LoadVideo(VideoClip videoClip)
    {
        foreach (var gameObject in listToDisable)
        {
            gameObject.SetActive(false);
        }

        videoPlayer.clip = videoClip;
        videoPlayer.Play();
    }

    private void UnloadVideo()
    {
        if (!videoPlayer.isPlaying)
        {
            foreach (var gameObject in listToEnable)
            {
                gameObject.SetActive(true);
            }

            videoPlayer.Stop();
            videoPlayer.clip = null;
        }
    }
}