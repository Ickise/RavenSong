using UnityEngine;

public class StunDetection : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private Collider2D stunCollider2D;
    private Collider2D ennemiCollider2D; // ici il faudra mettre le component qu'on veut déssactiver pour arrêter les mouvements etc...

    [SerializeField] private float stunDuration = 1.5f;
    [SerializeField] private float crossKickCooldown = 2f;
    [SerializeField] private float timeToDisableStunCollider = 0.3f;
    
    [Header("Ne pas set up")]
    [SerializeField] private float timeToCrossKick;
    [SerializeField] private float timeToEnableStunCollider;
    [SerializeField] private float timeToStun;

    private bool canLaunchTimeToStun;
    private bool launchTimeToDisableStunCollider;

    private void Update()
    {
        timeToCrossKick += Time.deltaTime;
        
        if (InputReader.instance.canStun && timeToCrossKick >= crossKickCooldown)
        {
            stunCollider2D.enabled = true;
            canLaunchTimeToStun = true;
            
            timeToCrossKick = 0;
        }

        if (canLaunchTimeToStun)
        {
            timeToEnableStunCollider += Time.deltaTime;
            timeToStun += Time.deltaTime;
            
            if (timeToEnableStunCollider >= timeToDisableStunCollider )
            {
                stunCollider2D.enabled = false;
                timeToEnableStunCollider = 0;
            }
            
            if (timeToStun >= stunDuration)
            {
                if (ennemiCollider2D != null)
                {
                    ennemiCollider2D.enabled = true; // bien mettre le bon component
                }
                
                timeToStun = 0;
                timeToEnableStunCollider = 0;
                canLaunchTimeToStun = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ennemiCollider2D = other;
        
        if (ennemiCollider2D != null)
        {
            ennemiCollider2D.enabled = false; // à modifier pour mettre le bon component 
        }
    }
}