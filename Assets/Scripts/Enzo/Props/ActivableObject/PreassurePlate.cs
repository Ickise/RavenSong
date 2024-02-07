using UnityEngine;

public class PreassurePlate : MonoBehaviour
{
    public bool isActive;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        isActive = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isActive = false;
    }
}