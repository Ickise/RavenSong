using UnityEngine;

public class StunDetection : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private float stunDuration = 1.5f;
    [SerializeField] private float crossKickCooldown = 2f;
    [SerializeField] private float timeToDisableRaycast = 0.3f;
    [SerializeField] private float distance = 0.5f;

    [SerializeField] private LayerMask layerMask;

    [Header("Ne pas set up")]
    [SerializeField] private float timeToCrossKick;
    [SerializeField] private float timeToEnableRaycast;
    [SerializeField] private float timeToStun;
    
    private IA _ia;

    private RaycastHit2D raycastHit2D;

    private bool canLaunchTimeToStun;
    private bool stopTimeToEnableRaycast;
    
    private void Update()
    {
        StunEnemy();
    }

    private void StunEnemy()
    {
        timeToCrossKick += Time.deltaTime;

        if (InputReader.instance.canStun && timeToCrossKick >= crossKickCooldown)
        {
            timeToCrossKick = 0;
            
            canLaunchTimeToStun = true;
            stopTimeToEnableRaycast = true;
            
            raycastHit2D = Physics2D.Raycast(transform.position, Vector2.right, distance, layerMask);
            if(raycastHit2D.collider.GetComponent<IA>() != null) _ia = raycastHit2D.collider.GetComponent<IA>(); //fait une seule erreur s'il ne détecte rien, à voir

        }

        if (canLaunchTimeToStun)
        {
            if (stopTimeToEnableRaycast) timeToEnableRaycast += Time.deltaTime;

            timeToStun += Time.deltaTime;
            
            if(_ia != null) _ia.enabled = false;
            
            if (timeToEnableRaycast >= timeToDisableRaycast)
            {
                raycastHit2D = new RaycastHit2D();

                timeToEnableRaycast = 0;
                stopTimeToEnableRaycast = false;
            }
            
            if (timeToStun >= stunDuration)
            {
                if (_ia != null)
                {
                    _ia.enabled = true;
                    _ia = null;
                }
                
                timeToStun = 0;
                canLaunchTimeToStun = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (stopTimeToEnableRaycast)
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * distance);
        }
    }
}