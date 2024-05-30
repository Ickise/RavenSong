using UnityEngine;

public class BalancingChandelier : MonoBehaviour
{
    [SerializeField, Header("Value"), Tooltip("Amplitude de la rotation")]
    private float rotationAmplitude = 0.01f;

    [SerializeField, Tooltip("Amplitude du mouvement")]
    private float movementAmplitude = 0.1f;

    [SerializeField] private float speed = 1f;

    [SerializeField, Header("Bool"), Tooltip("Mettre true si on veut que la position change")]
    private bool movePosition = true;

    [SerializeField, Tooltip("Mettre true si on veut que la rotation change")]
    private bool moveRotation = true;

    private float initialZ;
    private float initialX;
    private float elapsedTime = 0f;

    void Start()
    {
        initialZ = transform.localRotation.z;
        initialX = transform.position.x;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float newZ = initialZ + Mathf.Sin(elapsedTime * speed) * rotationAmplitude;
        float newX = initialX + Mathf.Sin(elapsedTime * speed) * movementAmplitude;

        if (moveRotation)
        {
            transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, newZ,
                transform.localRotation.w);
        }

        if (movePosition)
        {
            transform.position = new Vector2(newX, transform.position.y);
        }
    }
}