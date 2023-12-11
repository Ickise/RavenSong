using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Escalier : MonoBehaviour
{
    [SerializeField] private Transform plateforme, downPoint, upPoint;
    private bool onPlateform;
    // public static bool CanJumpEscalier { get { return  && yInput == -1; } }
    public static bool isOnEscalier = false;
    private float yInput;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (yInput < 0) return;
        if (other.CompareTag("Player") && other.transform.position.y > plateforme.position.y)
        {
            plateforme.gameObject.SetActive(true);
            onPlateform = true;
            isOnEscalier = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 1f - ((upPoint.position.x - other.transform.position.x) / (upPoint.position.x - downPoint.position.x))));
        if (Mathf.Abs(plateforme.position.y - other.transform.position.y) < 1.5f && onPlateform)
        {
            other.transform.position = new Vector2(other.transform.position.x, plateforme.position.y + 1.12f);
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
    }

    public void PassDown(InputAction.CallbackContext context)
    {
        yInput = context.ReadValue<Vector2>().y;
        if (yInput == -1)
        {
            plateforme.gameObject.SetActive(false);
            onPlateform = false;
        }
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // if (yInput == -1)
            //     plateforme.gameObject.SetActive(false);
            onPlateform = false;
        }
        if (context.canceled)
            onPlateform = true;
    }
}
