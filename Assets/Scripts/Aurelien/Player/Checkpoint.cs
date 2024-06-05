using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Checkpoint : MonoBehaviour
{
    public static List<Checkpoint> listOfActivCheckpoint = new List<Checkpoint>();
    private static Checkpoint currentPos;

    private Image interactionSprite;

    private VisualEffect vfx;
    private string checkpointColorProperty = "CheckpointColor";
    private string checkpointGradientProperty = "CheckpointGradient";


    [SerializeField] private Gradient newGradient;

    [SerializeField] private int checkpointOrder;

    private void Awake()
    {
        vfx = GetComponentInChildren<VisualEffect>();
        interactionSprite = GetComponentInChildren<Image>();

        if (listOfActivCheckpoint.Count >= checkpointOrder)
        {
            KeepCheckpoint();
        }
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
        Destroy(interactionSprite.gameObject);
    }

    public void AddElementAtList()
    {
        if (listOfActivCheckpoint.Count >= checkpointOrder) return;
        if (listOfActivCheckpoint.Contains(currentPos)) return;
        listOfActivCheckpoint.Add(currentPos);
    }

    private void KeepCheckpoint()
    {
        Destroy(interactionSprite.gameObject);
        Destroy(GetComponent<ShowMakeInteraction>());
        Destroy(GetComponent<Collider2D>());
        vfx.SetGradient(checkpointColorProperty, newGradient);
        vfx.SetGradient(checkpointGradientProperty, newGradient);
    }
}