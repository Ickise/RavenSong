using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class ControlsParameter : MonoBehaviour
{
    public static bool controllerVibration = true;
    private static PlayerIndex playerIndex;
    private bool inverseAxe;
    [SerializeField] private Sprite emptyCase, crossCase;
    [SerializeField] private GameObject inverseAxePar, vibrationPar;

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

    /// <summary>
    /// fait une vibration de la manette pendant une certaine durée
    /// </summary>
    public static void GamePadVibration(MonoBehaviour monoBehaviour, float powerLeft, float powerRight, float time)
    {
        if (!controllerVibration || monoBehaviour == null) return;
        GamePad.SetVibration(playerIndex, powerLeft, powerRight);
        monoBehaviour.StartCoroutine(EndVibration());
        IEnumerator EndVibration()
        {
            yield return new WaitForSeconds(time);
            GamePad.SetVibration(playerIndex, 0f, 0f);
        }
    }
    
    public void DefaultControlsSettings()
    {
        controllerVibration = false;
        inverseAxe = true;
    }
}
