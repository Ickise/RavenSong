using UnityEngine;

public class CameraPointMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float smoothSpeed = 2f;
    [SerializeField, Range(0f, 10f)] private float maxDistance = 7f;
    [SerializeField, Range(0f, 6f), Tooltip("La position en Y du GameObject CameraPointToFollow")] private float yCameraPoint = 3f;

    private Vector2 velocity;

    private void Update()
    {
        if (Mathf.Abs(InputReader.instance.direction.x) >= 0.03f)
        {
            SmoothCameraPointMovement();
        }
        else
        {
            transform.position = Vector2.SmoothDamp(transform.position,
                new Vector2(PlayerController2D._instance.transform.position.x, transform.position.y), ref velocity,
                1 / smoothSpeed);
        }

        if (InputReader.instance.activateAim)
        {
            //  LeftStickMoveCamera();
        }
    }

    private void SmoothCameraPointMovement()
    {
        Vector2 playerPosition = PlayerController2D._instance.transform.position;
        float destinationX = playerPosition.x + InputReader.instance.direction.x * maxDistance;

        Vector2 destination = new Vector2(destinationX, playerPosition.y + yCameraPoint);

        transform.position = Vector2.SmoothDamp(transform.position, destination, ref velocity, 1 / smoothSpeed);
    }

    private void LeftStickMoveCamera()
    {
        transform.position += InputReader.instance.manetteDirection;
    }
}