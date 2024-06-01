using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShowControl : MonoBehaviour
{
    [SerializeField, Header("Color")] private Color targetColor;

    [SerializeField] private GameObject[] listToEnable;
    [SerializeField] private Image[] imageListToEnable;

    private Color initialColor;

    private bool isTargetColorReached = false;

    private SpriteRenderer spriteRenderer;

    [SerializeField, Range(0.1f, 1f)] private float delay = 0.5f;

    private void Awake()
    {
        foreach (var gameObject in listToEnable)
        {
            gameObject.SetActive(false);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
        StartCoroutine(OnStart());
    }

    private IEnumerator OnStart()
    {
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * delay;
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, timer);
            yield return null;
        }

        yield return new WaitUntil(() => spriteRenderer.color == targetColor);
        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * delay;
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, initialColor, timer);
            yield return null;
        }

        yield return new WaitUntil(() => spriteRenderer.color == initialColor);

        foreach (var gameObject in listToEnable)
        {
            gameObject.SetActive(true);
        }

        foreach (var spriteRenderer in imageListToEnable)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, timer);

            if (spriteRenderer.color == targetColor)
            {
                isTargetColorReached = true;
            }
        }

        yield return new WaitUntil(() => isTargetColorReached);

        Destroy(gameObject);
    }
}