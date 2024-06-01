using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    private static Checkpoint currentPos;

    private Image interactionSprite;

    private void Awake()
    {
        if (currentPos != null && currentPos != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
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
        Destroy(interactionSprite);
    }
}