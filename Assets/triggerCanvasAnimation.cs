using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class triggerCanvasAnimation : MonoBehaviour
{
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] private Image targetImage;
    [SerializeField] private float fadeDuration = 2f;

    void Start()
    {
        Color color = targetImage.color;
        color.a = 0f;
        targetImage.color = color;

        StartCoroutine(ActivateAndFadeIn());
    }

    IEnumerator ActivateAndFadeIn()
    {
        targetGameObject.SetActive(true);
        InputReader.instance.enabled = false;
        InputReader.instance.GetComponent<PlayerInput>().enabled = false;
        
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f);
    }

    void SetAlpha(float alpha)
    {
        Color color = targetImage.color;
        color.a = alpha;
        targetImage.color = color;
    }
}
