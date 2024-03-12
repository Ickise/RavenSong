using UnityEngine;
using UnityEngine.InputSystem;

public class BulletDirection : MonoBehaviour
{
    [Header("Pas à set up")] public Vector3 direction;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 positionToLook = mainCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x,
                Mouse.current.position.ReadValue().y, 1));

            direction = SpineAim.manette
                ? InputReader.instance.manetteDirection
                : (positionToLook - PlayerController2D._instance.transform.position).normalized;
        }
    }
}