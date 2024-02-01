using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
   public bool isActive;

   private bool isPlayerInRange;

   private void Start()
   {
      InputReader.instance.onInteractionEvent.AddListener(OnClick);
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         isPlayerInRange = true;
      }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      isPlayerInRange = false;
   }

   private void OnClick()
   {
      if (isPlayerInRange)
      {
         if (!isActive)
         {
            isActive = true;
         }
         else
         {
            isActive = false;
         }
      }
   }
}
