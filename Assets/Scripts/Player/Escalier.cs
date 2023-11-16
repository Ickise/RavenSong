using UnityEngine;
using UnityEngine.InputSystem;

public class Escalier : MonoBehaviour
{
    [SerializeField] private Transform plateforme, downPoint, upPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform.position.y > plateforme.position.y)
            plateforme.gameObject.SetActive(true);

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 1f - ((upPoint.position.x - other.transform.position.x) / (upPoint.position.x - downPoint.position.x))));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            plateforme.gameObject.SetActive(false);
            plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 0));
        }
    }

    public void PassDown(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y < 0)
            plateforme.gameObject.SetActive(false);
    }
}
