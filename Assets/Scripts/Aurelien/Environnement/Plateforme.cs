using UnityEngine;

public class Plateforme : MonoBehaviour
{
    [SerializeField] private GameObject plateformeCollider;
    
    public static bool isOnPlateforme = false;

    private void Update()
    {
        if (isOnPlateforme && InputReader.instance.canDown)
        {
            plateformeCollider.SetActive(false);
            isOnPlateforme = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        plateformeCollider.SetActive(true);
        isOnPlateforme = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        plateformeCollider.SetActive(false);
        isOnPlateforme = false;
    }
}