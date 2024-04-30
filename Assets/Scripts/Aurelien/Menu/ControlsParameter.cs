using UnityEngine;
using UnityEngine.UI;

public class ControlsParameter : MonoBehaviour
{
    public static bool controllerVibration = true;
    private bool inverseAxe;
    [SerializeField] private Sprite emptyCase, crossCase;

    public void ChangeControllerVibration(Image image)
    {
        controllerVibration = !controllerVibration;
        image.sprite = controllerVibration ? crossCase : emptyCase;
    }

    public void InverseAxe(Image image)
    {
        inverseAxe = !inverseAxe;
        image.sprite = inverseAxe ? crossCase : emptyCase;
    }
}
