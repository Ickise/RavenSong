using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class triggerCanvasAnimation : MonoBehaviour
{
    // Référence au GameObject à activer
    public GameObject targetGameObject;
    // Référence à l'Image UI
    public Image targetImage;
    // Durée de l'animation de fondu
    public float fadeDuration = 2f;

    void Start()
    {
        // Initialiser l'alpha de l'image à 0
        Color color = targetImage.color;
        color.a = 0f;
        targetImage.color = color;

        // Commence la coroutine d'activation et de fondu
        StartCoroutine(ActivateAndFadeIn());
    }

    IEnumerator ActivateAndFadeIn()
    {
        // Activer le GameObject
        targetGameObject.SetActive(true);

        // Variable pour suivre le temps écoulé
        float elapsedTime = 0f;

        // Boucle jusqu'à ce que la durée de fondu soit atteinte
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        // Assurer que l'alpha est à 1 à la fin
        SetAlpha(1f);
    }

    void SetAlpha(float alpha)
    {
        Color color = targetImage.color;
        color.a = alpha;
        targetImage.color = color;
    }
}
