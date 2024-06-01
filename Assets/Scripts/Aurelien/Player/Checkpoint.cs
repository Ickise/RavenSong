using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Checkpoint : MonoBehaviour
{
    private static Checkpoint currentPos;

    private Image interactionSprite;

    private VisualEffect vfx;
    private string checkpointColorProperty = "CheckpointColor";
    private string checkpointGradientProperty = "CheckpointGradient";


    [SerializeField] private Gradient newGradient;

    private void Awake()
    {
        if (currentPos != null && currentPos != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        interactionSprite = GetComponentInChildren<Image>();
        vfx = GetComponentInChildren<VisualEffect>();
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

        vfx.SetGradient(checkpointColorProperty, newGradient);
        vfx.SetGradient(checkpointGradientProperty, newGradient);
        Destroy(interactionSprite);
    }
}