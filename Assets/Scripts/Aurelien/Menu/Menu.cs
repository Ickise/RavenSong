using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class Menu : MonoBehaviour
{
    [SerializeField] private SceneAsset newGameScene;
    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene.ToString());
    }

    public void Quit()
    {
        Application.Quit();
    }
}
