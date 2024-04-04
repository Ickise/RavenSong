using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTransition : MonoBehaviour
{
    [SerializeField] private float fadeTransitionTime = 0.5f;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    public void DoFadeTransitionFrom(GameObject transitionFrom)
    {
        CanvasGroup canvasGroup = transitionFrom.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0, fadeTransitionTime)
        .OnComplete(() => transitionFrom.SetActive(false));
    }

    public void DoFadeTransitionTo(GameObject transitionTo)
    {
        transitionTo.SetActive(true);
        CanvasGroup canvasGroup = transitionTo.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, fadeTransitionTime)
        .OnComplete(() => transitionTo.SetActive(true));
        eventSystem.SetSelectedGameObject(transitionTo.GetComponentInChildren<Button>().gameObject, new BaseEventData(eventSystem));
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject, new BaseEventData(eventSystem));
    }
}
