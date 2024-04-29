using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    // [SerializeField] private SceneAsset newGameScene;
    [SerializeField] private GameObject firstButtonSelected;

    private void Start()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(firstButtonSelected, new BaseEventData(eventSystem));
    }

    public void NewGame()
    {
        SceneManager.LoadScene("TestLevelDesign");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
