using UnityEngine;

public class CameraPointMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 5f), Header("Speed"),
     Tooltip("Modifie la vitesse du SmoothDamp lorsque le joueur se déplace")]
    private float basicMovementSpeed = 1f;
    [SerializeField, Range(0f, 5f)] private float basicMovementSpeed2 = 0.5f;

    [SerializeField, Range(0f, 5f), Tooltip("Modifie la vitesse du SmoothDamp lorsque le joueur vise")]
    private float aimSpeed = 2f;

    [SerializeField, Range(0f, 10f), Header("Distance"),
     Tooltip("Modifie la distance maximum depuis le joueur jusqu'où la caméra peut aller lorsque le joueur se déplace")]
    private float basicMovementMaxDistance = 4f;

    [SerializeField, Range(0f, 20f),
     Tooltip("Modifie la distance maximum depuis le joueur jusqu'où la caméra peut aller lorsque le joueur vise")]
    private float aimMaxDistance = 10f;

    [SerializeField, Tooltip("Ce float est la valeur ajouter au Y de la position du transform du player"),
     Range(0.1f, 5f), Header("Float Y Position")]
    private float floatToAdd;

    private Vector2 velocity;
    private Vector2 playerPosition;

    private void Update()
    {
        playerPosition = transform.parent.position;

        if (Mathf.Abs(InputReader.instance.direction.x) >= 0.03f)
        {
            InputReader.instance.activateAim = false;
        }

        if (InputReader.instance.activateAim)
        {
            AimWithLeftStick();
            return;
        }

        if (Mathf.Abs(InputReader.instance.direction.x) >= 0.03f)
        {
            SmoothCameraPointMovement();
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    private void SmoothCameraPointMovement()
    {
        float destinationX = playerPosition.x + InputReader.instance.direction.x * basicMovementMaxDistance;
        float destinationY = playerPosition.y + floatToAdd;

        Vector2 destination = new Vector2(destinationX, destinationY);

        transform.position =
            Vector2.SmoothDamp(transform.position, destination, ref velocity, 1 / basicMovementSpeed);
    }

    private void ReturnToOriginalPosition()
    {
        Vector2 target = new Vector2(playerPosition.x + 1F, playerPosition.y + 1);

        transform.position =
            Vector2.SmoothDamp(transform.position, target, ref velocity, 1 / basicMovementSpeed2);
    }

    private void AimWithLeftStick()
    {
        Vector2 target = InputReader.instance.manetteDirection * aimMaxDistance;
        transform.localPosition =
            Vector2.SmoothDamp(transform.localPosition, target, ref velocity, 1 / aimSpeed);
    }
}