using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Escalier : MonoBehaviour
{
    [SerializeField] private Transform plateforme, downPoint, upPoint;
    private bool canBeOnPlateform;
    public static bool isOnEscalier = false;
    private float yInput;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform.position.y > plateforme.position.y)
        {
            plateforme.gameObject.SetActive(true);
            canBeOnPlateform = true;
            isOnEscalier = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 1f - ((upPoint.position.x - other.transform.position.x) / (upPoint.position.x - downPoint.position.x))));
        if (Mathf.Abs(plateforme.position.y - other.transform.position.y) < 1.4f && canBeOnPlateform)
        {
            other.transform.position = new Vector2(other.transform.position.x, plateforme.position.y + 1.1f);
            isOnEscalier = true;
        }
        else
            isOnEscalier = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        plateforme.gameObject.SetActive(false);
        plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 0));
        isOnEscalier = false;
        canBeOnPlateform = false;
    }

    public void PassDown(InputAction.CallbackContext context)
    {
        yInput = context.ReadValue<Vector2>().y;
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (context.started && isOnEscalier)
        {
            if (yInput == -1)
                plateforme.gameObject.SetActive(false);
            canBeOnPlateform = false;
            StartCoroutine(OnplateformTrue());
            IEnumerator OnplateformTrue()
            {
                yield return new WaitForSeconds(0.2f);
                canBeOnPlateform = true;
            }
        }
    }
}
