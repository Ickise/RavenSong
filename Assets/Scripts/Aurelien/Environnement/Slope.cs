using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using DG.Tweening;

public class Slope : MonoBehaviour
{
    [SerializeField] private Transform plateforme, downPoint, upPoint;
    private PlayerController2D _player;
    private bool rightDirection;
    public static bool isOnSlope = false;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController2D>();
        if (upPoint.position.x > downPoint.position.x)
            rightDirection = true;
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player") && other.transform.position.y > plateforme.position.y)
    //     {
    //         plateforme.gameObject.SetActive(true);
    //         isOnSlope = false;
    //     }
    // }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 1f - ((upPoint.position.x - other.transform.position.x) / (upPoint.position.x - downPoint.position.x))));
        // if (Mathf.Abs(plateforme.position.y - other.transform.position.y) < 1.3f && plateforme.gameObject.activeInHierarchy)
        // {
        //     other.transform.position = new Vector2(other.transform.position.x, plateforme.position.y + 1f);
        //     isOnSlope = true;
        // }
        // else
        //     isOnSlope = false;
        _player.PlayerVelocity *= Vector2.right;
        if (InputReader.instance.direction != Vector2.zero && rightDirection ? _player.PlayerVelocity.x < -0.1f : _player.PlayerVelocity.x > 0.1f)
            _player.PlayerVelocity = new Vector2(_player.PlayerVelocity.x, -Mathf.Abs(_player.PlayerVelocity.x));
        else 
            _player.PlayerVelocity = Vector2.zero;
    }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (!other.CompareTag("Player")) return;
    //     plateforme.gameObject.SetActive(false);
    //     plateforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 0));
    //     isOnSlope = false;
    // }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (context.started && isOnSlope && plateforme.gameObject.activeInHierarchy)
        {

        }
    }
}
