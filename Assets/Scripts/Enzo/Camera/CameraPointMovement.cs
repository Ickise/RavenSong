using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraPointMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 5f), Header("Speed")]
    private float smoothSpeed = 2f;

    [SerializeField, Range(0f, 10f), Header("Distance")]
    private float maxDistance = 7f;

    [SerializeField, Range(-3f, 3f),
     Tooltip("La position en X que prendra le GameObject CameraPointToFollow"),
     Header("CameraPointToFollow Position")]
    private float positionX = 0f;

    [SerializeField, Range(0f, 5f),
     Tooltip("La position en Y que prendra le GameObject CameraPointToFollow")]
    private float positionY = 3f;

    private Vector2 velocity;

    private void Awake()
    {
        transform.localPosition = new Vector2(positionX, positionY);
    }

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

        Vector2 destination = new Vector2(destinationX, playerPosition.y + positionY);

        transform.position = Vector2.SmoothDamp(transform.position, destination, ref velocity, 1 / smoothSpeed);
    }

    private void LeftStickMoveCamera()
    {
        transform.position += InputReader.instance.manetteDirection;
    }
}