using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField, Header("Canvas"), Tooltip("Il faut mettre les GameObject importants du Canvas PauseMenuCanvas")]
    private GameObject[] pauseMenuElements;

    [SerializeField, Tooltip("Il faut mettre le GameObject Pause du Canvas PauseMenuCanvas")]
    private GameObject pauseMenu;

    public static PauseController _instance;

    [SerializeField] private Transform firstActiveGameObject;
    private GameObject firstSelectedGameObject;

    [SerializeField] private PlayerInput playerInput;

    private EventSystem eventSystem;

    //je ne peux pas réappuyer sur la touche du menu pause

    private void Awake()
    {
        _instance = this;
        DisableElements();

        eventSystem = EventSystem.current;
        firstSelectedGameObject = eventSystem.firstSelectedGameObject;
    }

    public void PauseUnPause()
    {
        GetActiveElement();

        if (firstActiveGameObject == null)
        {
            pauseMenu.SetActive(true);

            eventSystem.SetSelectedGameObject(firstSelectedGameObject);
            playerInput.SwitchCurrentActionMap("Menu");

            Time.timeScale = 0;
        }
        else
        {
            DisableElements();

            playerInput.SwitchCurrentActionMap("Player");

            Time.timeScale = 1;
        }
    }

    private void GetActiveElement()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf)
            {
                firstActiveGameObject = gameObject.transform.GetChild(i);
            }
        }
    }

    private void DisableElements()
    {
        foreach (var gameObject in pauseMenuElements)
        {
            gameObject.SetActive(false);
        }

        firstActiveGameObject = null;
    }
}