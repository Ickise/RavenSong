using UnityEngine;

public class DeathZone : MonoBehaviour
{
  private Respawn _respawn;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      _respawn = other.gameObject.GetComponentInParent<Respawn>();
      
      _respawn.RespawnPlayer();
    }
  }
}
