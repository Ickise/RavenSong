using UnityEngine;
using UnityEngine.InputSystem;

public class Escalier : MonoBehaviour
{
    [SerializeField] private Transform plateforme, downPoint, upPoint;
    private bool onPlateform;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform.position.y > plateforme.position.y)
        {
            plateforme.gameObject.SetActive(true);
            onPlateform = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 1f - ((upPoint.position.x - other.transform.position.x) / (upPoint.position.x - downPoint.position.x))));
        if (Mathf.Abs(plateforme.position.y - other.transform.position.y) < 1.5f && onPlateform)
            other.transform.position = new Vector2(other.transform.position.x, plateforme.position.y + 1.12f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        plateforme.gameObject.SetActive(false);
        plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 0));

    }

    public void PassDown(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y < 0)
        {
            plateforme.gameObject.SetActive(false);
            onPlateform = false;
        }
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (context.started)
            onPlateform = false;
        else if (context.canceled)
            onPlateform = true;
    }
}
