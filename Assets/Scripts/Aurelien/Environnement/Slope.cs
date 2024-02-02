using UnityEngine;
using UnityEngine.InputSystem;

public class Slope : MonoBehaviour
{
    [SerializeField] private Transform platforme, downPoint, upPoint;
    private PlayerController2D _player;
    private bool isJumping;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _player = other.GetComponentInParent<PlayerController2D>();
            platforme.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        // _player.PlayerVelocity = new Vector2(_player.PlayerVelocity.x, rightDirection ? _player.PlayerVelocity.x : -_player.PlayerVelocity.x);

        platforme.position = new Vector2(transform.position.x, Mathf.Lerp(downPoint.position.y, upPoint.position.y, 1f - ((upPoint.position.x - _player.transform.position.x) / (upPoint.position.x - downPoint.position.x))));
        if (isJumping) return;
        _player.transform.position = new Vector2(_player.transform.position.x, platforme.position.y + platforme.transform.localScale.y * 36.6f);
        // else
        //     isOnSlope = false;
        // _player.PlayerVelocity *= Vector2.right;
        // if (InputReader.instance.direction != Vector2.zero && rightDirection ? _player.PlayerVelocity.x < -0.1f : _player.PlayerVelocity.x > 0.1f)
        //     _player.PlayerVelocity = new Vector2(_player.PlayerVelocity.x, -Mathf.Abs(_player.PlayerVelocity.x));
        // else 
        //     _player.PlayerVelocity = Vector2.zero;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            platforme.gameObject.SetActive(false);
    }

    public void IsJumping(InputAction.CallbackContext context)
    {
        if (context.started)
            isJumping = true;
        else if (context.canceled)
            isJumping = false;
    }
}
