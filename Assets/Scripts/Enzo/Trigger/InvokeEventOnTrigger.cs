using UnityEngine;
using UnityEngine.Events;

public class InvokeEventOnTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEvent = new UnityEvent();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            onTriggerEvent.Invoke();
        }
    }
}
