using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject firstButtonSelected;

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
        Application.Quit();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}