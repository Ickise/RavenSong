using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Lever _lever;

    [SerializeField] private Collider2D doorCollider;

    private void Update()
    {
        OpenDoor(_lever, _lever.isActive);
    }

    private void OpenDoor(Component component, bool isActive)
    {
        if (component == null)
        {
            return;
        }
        
        if (isActive)
        {
            doorCollider.isTrigger = true;
        }
        else
        {
            doorCollider.isTrigger = false;
        }
    }
}