using UnityEngine;
using DG.Tweening;

public class MenuTransition : MonoBehaviour
{
    [SerializeField] private float fadeTransitionTime = 0.5f;

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
        canvasGroup.DOFade(1, fadeTransitionTime)
        .OnComplete(() => transitionTo.SetActive(true));
    }
}
