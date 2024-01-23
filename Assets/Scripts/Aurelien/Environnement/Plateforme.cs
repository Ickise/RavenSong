using UnityEngine;
using UnityEngine.InputSystem;

public class Plateforme : MonoBehaviour
{
    [SerializeField] private GameObject plateformeCollider;
    private float yInput;
    public static bool isOnPlateforme = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        plateformeCollider.SetActive(true);
        isOnPlateforme = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        plateformeCollider.SetActive(false);
        isOnPlateforme = false;
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
