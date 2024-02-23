using UnityEngine;
using Aurinaxtailer;
public class WeaponRotation : MonoBehaviour
{
    [Header("À set up")]
    [SerializeField] private bool isManette;
    public static bool manette;

    private void Start()
    {
        manette = isManette;
    }

    void Update()
    {
        if (manette && InputReader.instance.manetteDirection != Vector3.zero)
            transform.rotation = Rotation2D.LookToDirection2D(transform.rotation, InputReader.instance.manetteDirection);
        else if (!manette)
            Rotation2D.LookToMouse2DPerspective(transform);
    }
}