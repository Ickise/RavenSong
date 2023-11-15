using UnityEngine;
using UnityEngine.InputSystem;

public class Escalier : MonoBehaviour
{
    [SerializeField] private Transform plateforme, downPoint, upPoint;


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 1f - ((upPoint.position.x - other.transform.position.x) / (upPoint.position.x - downPoint.position.x))));
            print((upPoint.position.x - other.transform.position.x) / (upPoint.position.x - downPoint.position.x));
        }
    }
}
