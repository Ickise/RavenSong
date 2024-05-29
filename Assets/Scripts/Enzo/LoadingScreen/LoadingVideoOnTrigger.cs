using UnityEngine;
using UnityEngine.Events;

public class LoadingVideoOnTrigger : MonoBehaviour
{
    [Header("UnityEvent"),Tooltip("Mettre dans l'Event la fonction LoadVideo du script VideoLoader")]
    public UnityEvent onLoading = new UnityEvent();

    [HideInInspector] public bool canEnableObject;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onLoading.Invoke();
            canEnableObject = true;
            Destroy(gameObject);
        }
    }
}