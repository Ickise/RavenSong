using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class FlashingLight : MonoBehaviour
{
    [SerializeField, Header("List of different lights")]
    private Light2D[] listOfLight;

    [SerializeField, Range(0f, 10f), Header("Time to flash")]
    private float minimumTime = 0.1f;

    [SerializeField, Range(0f, 10f)] private float maximumTime = 1.9f;

    private float timer;
    private float randomTimeToFlash;

    private void Start()
    {
        randomTimeToFlash = Random.Range(minimumTime, maximumTime);
    }

    private void Update()
    {
        timer += 0.5f * Time.deltaTime;

        ShutLight();
        LightUp();
    }

    private void ShutLight()
    {
        if (timer <= randomTimeToFlash) return;

        foreach (var light2D in listOfLight)
        {
            light2D.enabled = false;
        }

        randomTimeToFlash = Random.Range(minimumTime, maximumTime);
        timer = 0;
    }

    private void LightUp()
    {
        if (timer <= 0.2f) return;
        
        foreach (var light2D in listOfLight)
        {
            light2D.enabled = true;
        }
    }
}