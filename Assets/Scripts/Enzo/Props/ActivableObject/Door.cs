using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Lever _lever;

    [SerializeField] private Collider2D doorCollider;

    private void Update()
    {
        OpenDoor(_lever, _lever.isActive);
    }

    private void OpenDoor(Lever lever, bool isActive)
    {
        if (lever == null)
        {
            return;
        }

        if (isActive)
        {
            doorCollider.isTrigger = true;
            //remplacer par une animation qui se lance et fait redescendre la porte ou bien un tween pour monter et descendre (cela permettra de prendre le collider ou
            //bien set up dans l'animation le collider qui bouge aussi
        }
        else
        {
            doorCollider.isTrigger = false;
            //pareil ici
        }
    }
}