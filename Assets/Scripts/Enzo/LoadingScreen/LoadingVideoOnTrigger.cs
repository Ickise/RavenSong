using UnityEngine;
using UnityEngine.Events;

public class LoadingVideoOnTrigger : MonoBehaviour
{
    public UnityEvent onLoading = new UnityEvent();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onLoading.Invoke();
        }
    }
}