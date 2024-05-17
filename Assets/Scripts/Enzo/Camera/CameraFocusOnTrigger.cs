using UnityEngine;
using UnityEngine.Events;

public class CameraFocusOnTrigger : MonoBehaviour
{
    [Header("UnityEvent"),Tooltip("Mettre dans l'Event la fonction CameraFocus du script CameraPointMovement")]
    public UnityEvent onFocusing = new UnityEvent();
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InputReader.instance.activateAim = false;
            onFocusing.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onFocusing.RemoveAllListeners();
        }
    }
}
