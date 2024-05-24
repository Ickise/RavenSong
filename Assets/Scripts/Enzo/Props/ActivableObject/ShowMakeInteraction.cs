using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShowMakeInteraction : MonoBehaviour
{
    [SerializeField, Header("Time"),
     Tooltip(
         "Ce float permet de changer le temps où il faut maintenir la touche pour interagir avec l'objet et lancer l'interaction")]
    private float timeToInteract = 1f;

    private Image interactionSprite;

    public UnityEvent onInteraction = new UnityEvent();

    private void Awake()
    {
        interactionSprite = GetComponentInChildren<Image>();
        interactionSprite.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionSprite.enabled = true;

            if (InputReader.instance.canInteract)
            {
                StartCoroutine(HoldActive());
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionSprite.enabled = false;
            InputReader.instance.canInteract = false;
        }
    }

    private IEnumerator HoldActive()
    {
        yield return new WaitForSeconds(timeToInteract);
        onInteraction.Invoke();
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponentInChildren<Canvas>().gameObject);
        Destroy(this);
    }
}