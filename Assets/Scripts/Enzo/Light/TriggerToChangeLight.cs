using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TriggerToChangeLight : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Color newColor = new Color(62, 79, 56, 255);

    [SerializeField, Range(0.1f, 1f)] private float delay = 0.5f;

    private float timer;

    private bool changeLight;

    [SerializeField,
     Tooltip(
         "Ce int correspond au nombre de checkpoint que le joueur a dû récupérer pour conserver les données de l'entry")]
    private int checkpointNumber;

    private void Start()
    {
        if (Checkpoint.listOfActivCheckpoint.Count < checkpointNumber) return;
        KeepChanges();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            changeLight = true;
        }
    }

    private void Update()
    {
        if (!changeLight) return;

        timer += Time.deltaTime * delay;
        ChangeLightColor();
    }

    private void ChangeLightColor()
    {
        globalLight.color = Color.Lerp(globalLight.color, newColor, timer);

        DestroyTrigger();
    }

    private void DestroyTrigger()
    {
        if (globalLight.color == newColor)
        {
            Destroy(gameObject);
        }
    }

    private void KeepChanges()
    {
        globalLight.color = newColor;
        DestroyTrigger();
    }
}