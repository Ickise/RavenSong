using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Ease ease;

    [SerializeField] private float newYPosition;
    [SerializeField] private float newXPosition;
    [SerializeField] private float animationDuration = 1f;

    [Tooltip(
        "Si ce bool est en true, il ne faut changer que newYPosition, s'il est false, il ne faut changer que newXPosition")]
    [SerializeField]
    private bool changeYorXPosition;

    private Vector3 originalPosition;
    private Vector3 newPosition;

    private void Awake()
    {
        originalPosition = transform.position;

        newPosition = changeYorXPosition
            ? new Vector3(transform.position.x, newYPosition, transform.position.z)
            : new Vector3(newXPosition, transform.position.y, transform.position.z);
    }

    public void OpenDoor(bool isActive)
    {
        if (isActive)
        {
            transform.DOMove(newPosition, animationDuration).SetEase(ease);
        }
        else
        {
            transform.DOMove(originalPosition, animationDuration).SetEase(ease);
        }
    }
}