using UnityEngine;
using UnityEngine.InputSystem;

public class Plateforme : MonoBehaviour
{
    [SerializeField] private GameObject plateformeCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        plateformeCollider.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        plateformeCollider.SetActive(false);
    }

    public void PassDown(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y == -1)
            plateformeCollider.SetActive(false);
    }
}
