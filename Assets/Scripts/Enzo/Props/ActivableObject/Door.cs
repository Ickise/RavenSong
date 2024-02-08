using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Ease ease;

    [SerializeField] private float newYPosition;
    [SerializeField] private float animationDuration = 1f;

    private Vector3 originalPosition;
    private Vector3 newPosition;

    private void Awake()
    {
        originalPosition = transform.position;

        newPosition = new Vector3(transform.position.x, newYPosition, transform.position.z);
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