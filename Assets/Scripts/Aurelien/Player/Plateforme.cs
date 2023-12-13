using UnityEngine;
using UnityEngine.InputSystem;

public class Plateforme : MonoBehaviour
{
    [SerializeField] private GameObject plateformeCollider;
    private float yInput;

    private void OnTriggerEnter2D(Collider2D other)
    {
        plateformeCollider.SetActive(true);
        Escalier.isOnEscalier = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        plateformeCollider.SetActive(false);
        Escalier.isOnEscalier = false;
    }

    public void PassDown(InputAction.CallbackContext context)
    {
        yInput = context.ReadValue<Vector2>().y;
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (context.started && yInput == -1 && plateformeCollider.activeInHierarchy)
                plateformeCollider.SetActive(false);
    }
}
