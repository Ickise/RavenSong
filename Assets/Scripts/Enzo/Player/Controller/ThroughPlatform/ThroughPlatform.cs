using UnityEngine;

public class ThroughPlatform : MonoBehaviour
{
    [SerializeField] private bool isUp;

    [SerializeField] private GameObject player;

    //  [SerializeField] private RaycastDetection _raycastDetection;
    // mettre dans une plateforme un collider, mettre deux gameobjects avec chacun un collider, il y en a un au dessus et l'autre en dessous (up/down) et activer le bool up pour celui du dessus.
// trouver un moyen de bloquer le jump direct après la plateforme.
    private void Update()
    {
        if (InputReader.instance.canDown && player.GetComponentInChildren<RaycastDetection>().isGrounded)
        {
//            player.GetComponentInChildren<RaycastDetection>().isGrounded ou _raycastDetection.isGrounded
            transform.parent.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<Collider2D>().enabled = isUp;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<Collider2D>().enabled = false;
        }
    }
}