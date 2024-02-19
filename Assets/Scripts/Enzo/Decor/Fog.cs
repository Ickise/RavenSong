using UnityEngine;
using UnityEngine.Events;

public class Fog : MonoBehaviour
{
    public UnityEvent onTriggerEvent = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerEvent.Invoke();
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}