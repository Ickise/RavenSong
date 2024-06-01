using System.Collections;
using TMPro;
using UnityEngine;

public class Announcement : MonoBehaviour
{
    private TextMeshProUGUI textToUpdate;

    [SerializeField, Header("Time"), Range(0.1f, 1f),
     Tooltip("Delay correspond au multiplicateur du timer, plus il est haut, plus le texte apparaît rapidement.")]
    private float delay = 0.5f;

    [SerializeField, Range(0f, 1f), Tooltip("Temps d'attente avant que le texte disparait")]
    private float waitToReset = 1f;

    [SerializeField, Header("Color")] private Color targetColor;

    private Color initialColor;

    private bool isTargetColorReached = false;

    private void Awake()
    {
        textToUpdate = GetComponent<TextMeshProUGUI>();
        initialColor = textToUpdate.color;
    }

    public void UpdateText(string announcementName)
    {
        textToUpdate.text = announcementName;
        StartCoroutine(FadeTextToTargetColor());
    }

    private IEnumerator FadeTextToTargetColor()
    {
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * delay;

            textToUpdate.color = Color.Lerp(initialColor, targetColor, timer);
            yield return null;
        }

        isTargetColorReached = true;
        yield return new WaitForSeconds(waitToReset);

        StartCoroutine(FadeTextToInitialColor());
    }

    private IEnumerator FadeTextToInitialColor()
    {
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * delay;
            textToUpdate.color = Color.Lerp(textToUpdate.color, initialColor, timer);
            yield return null;
        }

        isTargetColorReached = false;
    }
}