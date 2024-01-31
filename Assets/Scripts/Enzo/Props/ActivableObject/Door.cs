using System;
using UnityEngine;

public class Door : MonoBehaviour
{
  [SerializeField] private Lever _lever;

  [SerializeField] private Collider2D doorCollider;

  private void Update()
  {
    OpenDoor();
  }

  private void OpenDoor()
  {
    if (_lever.isActive)
    {
      doorCollider.isTrigger = true;
    }
  }
}
