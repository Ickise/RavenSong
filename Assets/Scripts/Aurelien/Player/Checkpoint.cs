using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activeCheckpoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Respawn.checkPoint = true;
            Respawn.spawnPosition = transform.position;
            Destroy(GetComponent<Collider2D>());
        }
    }
}
