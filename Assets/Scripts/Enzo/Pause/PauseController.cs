using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField, Header("Canvas"), Tooltip("Il faut mettre les GameObject importants du Canvas PauseMenuCanvas")]
    private GameObject[] pauseMenuElements;

    [SerializeField, Tooltip("Il faut mettre le GameObject Pause du Canvas PauseMenuCanvas")]
    private GameObject pauseMenu;

    public static PauseController _instance;

    private Transform firstActiveGameObject;
    private GameObject firstSelectedGameObject;
    
    public static bool gameIsPaused;

    [SerializeField] private GameObject selectionImage;

    [SerializeField] private float addingXPosition;

    private void Awake()
    {
        _instance = this;
        DisableElements();
    }

    public void PauseUnPause()
    {
        GetActiveElement();

        if (firstActiveGameObject == null)
        {
            pauseMenu.SetActive(true);
            //  selectionImage.SetActive(true);

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

    public void TabChanger()
    {
        switch (InputReader.instance.tabID)
        {
            case -1:
                DisableElements();
                pauseMenuElements[1].SetActive(true);
                break;
            case 0:
                DisableElements();
                pauseMenuElements[0].SetActive(true);
                break;
            case 1:
                DisableElements();
                pauseMenuElements[2].SetActive(true);
                break;
        }
    }
}