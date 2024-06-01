using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TriggerToChangeLight : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Color newColor = new Color(62, 79, 56, 255);

    [SerializeField, Range(0.1f, 1f)] private float delay = 0.5f;

    private float timer;

    private bool changeLight;

    private void Awake()
    {
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
        globalLight.color = new Color(Mathf.Lerp(globalLight.color.r, newColor.r, timer),
            Mathf.Lerp(globalLight.color.g, newColor.g, timer),
            Mathf.Lerp(globalLight.color.b, newColor.b, timer));
        
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
        if (!Respawn.alreadyDeath) return;
        
        globalLight.color = newColor;
        Destroy(gameObject);
    }
}