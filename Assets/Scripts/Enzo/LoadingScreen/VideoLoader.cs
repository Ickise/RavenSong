using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer), typeof(LoadingVideoOnTrigger))]
public class VideoLoader : MonoBehaviour
{
    [SerializeField, Header("GameObjectList")] private GameObject[] listToDisable;
    [SerializeField] private GameObject[] listToEnable;

    [SerializeField, Header("VideoPlayer")] private VideoPlayer videoPlayer;

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