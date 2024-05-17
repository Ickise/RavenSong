using UnityEngine;
using UnityEngine.UI;

public class ShowInteraction : MonoBehaviour
{
    private Image interactionSprite;

    private void Awake()
    {
        interactionSprite = GetComponentInChildren<Image>();
        interactionSprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionSprite.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionSprite.enabled = false;
        }
    }
}