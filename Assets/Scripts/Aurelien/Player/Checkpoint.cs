using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static bool canActiveCheckpoint;
    
    private static Checkpoint currentPos;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentPos = GetComponent<Checkpoint>();
        }
    }

    public void ActiveCheckpoint()
    {
        Respawn.checkPoint = true;
        Respawn.spawnPosition = currentPos.transform.position;

        currentPos.GetComponent<SpriteRenderer>().color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.11f);
    }
}