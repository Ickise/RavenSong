using UnityEngine;
using UnityEngine.InputSystem;

public class BulletDirection : MonoBehaviour
{
    [HideInInspector] public Vector3 direction;

    [SerializeField, Header("PositionToLook.z"), Range(0.1f, 20f),
     Tooltip("Mettre la même valeur que la caméra distance dans la section body de la caméra")]
    private float cameraDistance = 10f;
    
    private Vector3 previousManetteDirection;

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
                Mouse.current.position.ReadValue().y, cameraDistance));
            
            direction = SpineAim.manette
                ? InputReader.instance.manetteDirection
                : (positionToLook - FireOneBullet.instance.ShootPosition.position).normalized;

            previousManetteDirection = InputReader.instance.manetteDirection;
            
            /*if (InputReader.instance.manetteDirection != Vector3.zero)
            {
               
            }
            else
            {
                direction = previousManetteDirection;
            }*/
        }
    }
}