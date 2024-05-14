using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    private static bool canActiveCheckpoint;
    
    private static Checkpoint currentPos;
    
    [SerializeField] private float timeActiveCheckpoint = 2;

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
            canActiveCheckpoint = true;
            
            currentPos = GetComponent<Checkpoint>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionSprite.enabled = false;
            canActiveCheckpoint = false;
        }
    }

    public void ActiveCheckpoint(InputAction.CallbackContext context)
    {
        if (!canActiveCheckpoint) return;
        if (context.started)
            currentPos.StartCoroutine(HoldActive());
        else if (context.canceled)
            currentPos.StopAllCoroutines();
    }

    private IEnumerator HoldActive()
    {
        yield return new WaitForSeconds(timeActiveCheckpoint);
        Respawn.checkPoint = true;
        Respawn.spawnPosition = currentPos.transform.position;
        Destroy(currentPos.GetComponent<Collider2D>());

        currentPos.GetComponent<SpriteRenderer>().color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.11f);
    }
}