using UnityEngine;

public class StunDetection : MonoBehaviour
{
    [Header("À set up")] [SerializeField] private float stunDuration = 1.5f;
    [SerializeField] private float crossKickCooldown = 2f;
    [SerializeField] private float timeToDisableRaycast = 0.3f;
    [SerializeField] private float distanceToHit = 0.5f;

    [SerializeField] private LayerMask layerIa;

    private float timeToCrossKick;
    private float timeToEnableRaycast;
    private float timeToStun;

    private IA _ia;
    private PlayerController2D _playerController2D;

    private RaycastHit2D raycastHit2D;

    private bool canLaunchTimeToStun;
    private bool stopTimeToEnableRaycast;

    private void Start()
    {
        _playerController2D = GetComponentInParent<PlayerController2D>();
    }

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

            raycastHit2D = Physics2D.Raycast(transform.position, Vector2.right * _playerController2D.LastDirection,
                distanceToHit, layerIa);

            if (raycastHit2D)
            {
                if (raycastHit2D.transform.GetComponent<IA>())
                    _ia = raycastHit2D.transform.GetComponent<IA>();
                else if (raycastHit2D.transform.CompareTag("DestroyObject"))
                {
                    Explodable explodableObj = raycastHit2D.transform.GetComponent<Explodable>();
                    explodableObj.explode();
                    ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
                    ef.doExplosion(transform.position);
                }
            }
        }

        if (canLaunchTimeToStun)
        {
            if (stopTimeToEnableRaycast) timeToEnableRaycast += Time.deltaTime;

            timeToStun += Time.deltaTime;

            if (_ia != null) _ia.enabled = false;

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
}