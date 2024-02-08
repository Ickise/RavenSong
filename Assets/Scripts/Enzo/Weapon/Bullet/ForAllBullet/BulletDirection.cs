using UnityEngine;

public class BulletDirection : MonoBehaviour
{
    private Vector3 mousePosition;

    [Header("Pas à set up")] public Vector3 direction;
    
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - transform.position;
    }
}