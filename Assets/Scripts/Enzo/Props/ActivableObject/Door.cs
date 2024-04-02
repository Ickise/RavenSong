using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField, Header("TweenMovement")] private Ease ease;

    [SerializeField, Header("NewPosition")] private float newYPosition;
    [SerializeField] private float newXPosition;
    [SerializeField, Header("AnimationDuration"), Range(0f, 2f)] private float animationDuration = 1f;

    [SerializeField, Tooltip(
         "Si ce bool est en true, il ne faut changer que newYPosition, s'il est false, il ne faut changer que newXPosition"), Header("Bool")]
    private bool changeYorXPosition;

    private Vector3 originalPosition;
    private Vector3 newPosition;
    [SerializeField] private GameObject vfxOpenDoor;

    private void Awake()
    {
        originalPosition = transform.position;

        newPosition = changeYorXPosition
            ? new Vector3(transform.position.x, newYPosition, transform.position.z)
            : new Vector3(newXPosition, transform.position.y, transform.position.z);
    }

    public void OpenDoor(bool isActive)
    {
        VFXInstantieur.instance.PlayVFXInWorld(vfxOpenDoor, transform);
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