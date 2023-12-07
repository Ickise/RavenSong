using UnityEngine;

public class ThroughPlatform : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private RaycastDetection _raycastDetection;
    
    private void Update()
    {
        if (InputReader.instance.canDown && _raycastDetection.isGrounded)
        {
            transform.parent.GetComponent<Collider2D>().enabled = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<Collider2D>().enabled = true;
        }
    }
}