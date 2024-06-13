using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    [SerializeField, Header("GameObjectList")]
    private GameObject[] listToDisable;

    [SerializeField] private GameObject[] listToEnable;

    [SerializeField, Header("VideoPlayer")]
    private VideoPlayer cinematicCameraVideoPlayer;

    [SerializeField, Header("Prefab CinematicCamera")]
    private GameObject cinematicCamera;

    private LoadingVideoOnTrigger _loadingVideoOnTrigger;

    private void Awake()
    {
        _loadingVideoOnTrigger = GetComponent<LoadingVideoOnTrigger>();
    }

    private void Update()
    {
        if (!_loadingVideoOnTrigger.canEnableObject) return;
        UnloadVideo();
    }

    public void LoadVideo(VideoClip videoClip)
    {
        foreach (var gameObject in listToDisable)
        {
            gameObject.SetActive(false);
        }

        cinematicCamera.SetActive(true);
        cinematicCameraVideoPlayer.clip = videoClip;
        cinematicCameraVideoPlayer.Play();
    }

    private void UnloadVideo()
    {
        if (cinematicCameraVideoPlayer.isPlaying) return;

        cinematicCamera.SetActive(false);
        foreach (var gameObject in listToEnable)
        {
            gameObject.SetActive(true);
        }

        cinematicCameraVideoPlayer.Stop();
        cinematicCameraVideoPlayer.clip = null;
        _loadingVideoOnTrigger.canEnableObject = false;
        Destroy(gameObject);
    }
}