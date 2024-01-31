using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Lever _lever;
    [SerializeField] private PreassurePlate _preassurePlate;

    [SerializeField] private Collider2D doorCollider;

    private void Update()
    {
        OpenDoor(_preassurePlate, _preassurePlate?.isActive);
        OpenDoor(_lever, _lever?.isActive);
    }

    private void OpenDoor(Component component, bool? isActive)
    {
        if (component == null || !isActive.HasValue)
        {
            return;
        }

        bool isActiveValue = isActive.Value;

        if (isActiveValue)
        {
            doorCollider.isTrigger = true;
        }

        if (!_preassurePlate.isActive && !_lever.isActive)
        {
            doorCollider.isTrigger = false;
        }
    }
}