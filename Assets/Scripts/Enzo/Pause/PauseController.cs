using UnityEngine;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour
{
    [SerializeField, Header("Canvas"), Tooltip("Il faut mettre les GameObject importants du Canvas PauseMenuCanvas")]
    private GameObject[] pauseMenuElements;

    [SerializeField, Tooltip("Il faut mettre le GameObject Pause du Canvas PauseMenuCanvas")]
    private GameObject pauseMenu;

    public static PauseController _instance;

    private Transform firstActiveGameObject;
    private GameObject firstSelectedGameObject;

    private EventSystem eventSystem;

    public static bool gameIsPaused;

    [SerializeField] private GameObject selectionImage;

    [SerializeField] private float addingXPosition;

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
            //  selectionImage.SetActive(true);

            eventSystem.SetSelectedGameObject(firstSelectedGameObject);
            gameIsPaused = true;

            Time.timeScale = 0;
        }
        else
        {
            DisableElements();
            gameIsPaused = false;
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

        // selectionImage.SetActive(false);
        firstActiveGameObject = null;
    }

    private void Update()
    {
        /*   if (gameIsPaused)
           {
               Vector2 selectionImagePosition = new Vector2(eventSystem.currentSelectedGameObject.transform.position.x + addingXPosition, eventSystem.currentSelectedGameObject.transform.position.y);
   
               selectionImage.transform.position = selectionImagePosition;
           }
       }*/
    }

    public void TabChanger()
    {
        switch (InputReader.instance.tabID)
        {
            case -1:
                Debug.Log("test-1");
                break;
            case 0:
                Debug.Log("test0");
                break;
            case 1:
                Debug.Log("test1");
                break;
        }
    }
}