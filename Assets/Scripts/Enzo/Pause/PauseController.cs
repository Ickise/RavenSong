using UnityEngine;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseFirstButton, optionsFirstButton, optionsClosedButton;

    public static PauseController _instance;

    private void Awake()
    {
        _instance = this;
        pauseMenu.SetActive(false);
    }

    public void PauseUnPause()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
