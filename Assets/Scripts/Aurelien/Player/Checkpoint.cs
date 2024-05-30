using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    private static Checkpoint currentPos;
    
    private Image interactionSprite;

    private void Awake()
    {
        interactionSprite = GetComponentInChildren<Image>();
    }
    
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
        Destroy(interactionSprite);
    }
}