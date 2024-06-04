using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject firstButtonSelected;
    [SerializeField] private SoundData quitGame, menuIntro, menuLoop;
    [SerializeField] private string sceneNameToPlay;

    private void Start()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(firstButtonSelected, new BaseEventData(eventSystem));
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (menuIntro == null) return;

        AudioManager.instance.PlayMusic(menuIntro);
        float a = 0;
        DOTween.To(() => a, x => a = x, 1, menuIntro.AudioToPlay.length)
        .OnComplete(() => AudioManager.instance.PlayMusic(menuLoop));
    }

    public void NewGame()
    {
        Respawn.checkPoint = false;
        SceneManager.LoadScene(sceneNameToPlay);
    }

    public void Quit()
    {
        AudioManager.instance.PlaySound(quitGame);
        float a = 0;
        DOTween.To(() => a, x => a = x, 1, 0.5f)
        .OnComplete(() => Application.Quit());
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}