using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FillAmountImage : MonoBehaviour
{
    [SerializeField, Header("Float"), Tooltip("C'est la valeur de départ du Fill Amount"), Range(0f, 1f)]
    private float startingFillAmount;
    
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    
    public IEnumerator AnimateImageFill(float duration)
    {
        float elapsedTime = 0f;

        image.fillAmount = startingFillAmount;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            image.fillAmount = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
    }

    public void ResetFill()
    {
        image.fillAmount = startingFillAmount;
    }
}