using UnityEngine;
using UnityEngine.InputSystem;

public class BulletDirection : MonoBehaviour
{
    [Header("Pas à set up")] public Vector3 direction;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        Camera mainCamera = Camera.main;

        Transform rightAimPosition =
            PlayerController2D._instance.GetComponentInChildren<FireOneBullet>().rightAimPosition;

        Transform leftAimPosition =
            PlayerController2D._instance.GetComponentInChildren<FireOneBullet>().leftAimPosition;

        Vector3 currentDirection = PlayerController2D._instance.CurrentDirection > 0
            ? rightAimPosition.position
            : leftAimPosition.position;

        Vector3 positionToLook = mainCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x,
            Mouse.current.position.ReadValue().y, mainCamera.nearClipPlane));

        direction = SpineAim.manette ? (currentDirection - transform.position).normalized : (positionToLook - transform.position).normalized;
    }
}