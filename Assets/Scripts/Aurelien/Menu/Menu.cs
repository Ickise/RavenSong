using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject firstButtonSelected;
    [SerializeField] private SoundData quitGame;
    [SerializeField] private string sceneNameToPlay;

    private void Start()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(firstButtonSelected, new BaseEventData(eventSystem));
    }

    public void NewGame()
    {
        SceneManager.LoadScene(sceneNameToPlay);
    }

    public void Quit()
    {
        AudioManager.instance.PlaySound(quitGame);
        float a = 0;
        DOTween.To(() => a, x => a = x, 1, 0.5f);
        Application.Quit();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}