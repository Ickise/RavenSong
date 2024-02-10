using UnityEngine;
using UnityEngine.InputSystem;

public class BulletDirection : MonoBehaviour
{
    private Vector3 mousePosition;
    
    private Vector3 rotation;

    [Header("Pas à set up")] public Vector3 direction;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, Camera.main.nearClipPlane));
        direction = (mousePosition - transform.position).normalized;
    }
}