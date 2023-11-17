using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private float speedRotation = 10;
    
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.y));

        float rotationZ = Mathf.Atan2(worldPosition.y - transform.position.y, worldPosition.x - transform.position.x) * Mathf.Rad2Deg;

        Quaternion newRotation = Quaternion.Euler(0, 0, rotationZ);

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speedRotation * Time.deltaTime);
    }
}