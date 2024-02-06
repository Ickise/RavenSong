using UnityEngine;

public class BulletDirection : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 worldPosition;
    private Vector3 initialCharacterPosition;

    private  PlayerController2D _playerController;

    [Header("Pas à set up")] public Vector3 direction;

    private void Start()
    {
        Init();
    }

    void Update()
    {
        worldPosition =
            Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.y));

        direction = worldPosition - initialCharacterPosition;
    }

    public void Init()
    {
        _playerController = FindObjectOfType<PlayerController2D>();
        initialCharacterPosition = _playerController.gameObject.transform.position;

        mousePosition = Input.mousePosition;
    }
}