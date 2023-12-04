using UnityEngine;

public class BulletDirection : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 worldPosition;
    private Vector3 initialCharacterPosition;

    private WeaponRotation _weaponRotation;
    
    [Header("Pas à set up")]   
    public Vector3 direction;
    
    private void Start()
    {
        Init();
    }

    void Update()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.y));
        
        direction = worldPosition - initialCharacterPosition;
    }

    public void Init()
    {
        _weaponRotation = FindObjectOfType<WeaponRotation>();
        initialCharacterPosition = _weaponRotation.gameObject.transform.position;
        
        mousePosition = Input.mousePosition;
    }
}