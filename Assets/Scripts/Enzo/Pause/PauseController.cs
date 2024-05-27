using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField, Header("Canvas"), Tooltip("Il faut mettre les GameObject importants du Canvas PauseMenuCanvas")]
    private GameObject[] pauseMenuElements;

    [SerializeField, Tooltip("Il faut mettre le GameObject Pause du Canvas PauseMenuCanvas")]
    private GameObject pauseMenu;

    public static PauseController _instance;

    private EventSystem eventSystem;

    private Transform firstActiveGameObject;
    
    private void Awake()
    {
        _instance = this;
        DisableElements();

        eventSystem = EventSystem.current;
    }

    public void PauseUnPause()
    {
        GetActiveElement();

        if (firstActiveGameObject == null)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            DisableElements();
            InputReader.instance.enabled = true;
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

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null || !eventSystem.currentSelectedGameObject.activeInHierarchy)
        {
            eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject,
                new BaseEventData(eventSystem));
        }
    }
}