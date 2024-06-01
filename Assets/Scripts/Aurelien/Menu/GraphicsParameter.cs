using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GraphicsParameter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI qualityText, fullscreenModeText, resolutionText, brightessText;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Vector2Int[] resolutions;
    private int indexQuality, indexFullscreenMode, indexResolution;

    private void Start()
    {
        qualityText.text = "<size=150%> <</size>" + QualitySettings.names[indexQuality = QualitySettings.GetQualityLevel()] + "<size=150%>> </size>";
        fullscreenModeText.text = "<size=150%> <</size>Full-Screen<size=150%>> </size>";
        indexResolution = resolutions.Length - 1;
        resolutionText.text = "<size=150%> <</size>" + resolutions[indexResolution].x + " × " + resolutions[indexResolution].y + "<size=150%>> </size>";
        brightessText.SetText(Mathf.Round((brightnessSlider.value = Screen.brightness) * 10).ToString());
    }

    public void ManetteControl(InputAction.CallbackContext context)
    {
        if (!context.started || !gameObject.activeInHierarchy) return;
        int add = Mathf.RoundToInt(Mathf.Clamp(context.ReadValue<float>() * Mathf.Infinity, -1, 1));
        if (EventSystem.current.currentSelectedGameObject == qualityText.transform.parent.gameObject)
            ChangeQuality(add);
        else if (EventSystem.current.currentSelectedGameObject == fullscreenModeText.transform.parent.gameObject)
            ChangeFullscreenMode(add);
        else if (EventSystem.current.currentSelectedGameObject == resolutionText.transform.parent.gameObject)
            ChangeResolution(add);
        else if (EventSystem.current.currentSelectedGameObject == brightessText.transform.parent.gameObject)
        {
            brightnessSlider.value += add / 10f;
            ChangeBrightness();
        }
    }

    public void ChangeQuality(int add)
    {
        indexQuality += add;
        if (indexQuality >= QualitySettings.count)
            indexQuality = 0;
        else if (indexQuality < 0)
            indexQuality = QualitySettings.count - 1;
        QualitySettings.SetQualityLevel(indexQuality, true);
        qualityText.text = "<size=150%> <</size>" + QualitySettings.names[QualitySettings.GetQualityLevel()] + "<size=150%>> </size>";
    }

    public void ChangeFullscreenMode(int add)
    {
        indexFullscreenMode += add;
        if (indexFullscreenMode >= 3)
            indexFullscreenMode = 0;
        else if (indexFullscreenMode < 0)
            indexFullscreenMode = 2;
        if (indexFullscreenMode == 0)
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
            fullscreenModeText.text = "<size=150%> <</size>Full-Screen<size=150%>> </size>";
        }
        else if (indexFullscreenMode == 1)
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.MaximizedWindow);
            fullscreenModeText.text = "<size=150%> <</size>Borderless Window<size=150%>> </size>";
        }
        else if (indexFullscreenMode == 2)
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
            fullscreenModeText.text = "<size=150%> <</size>Windowed<size=150%>> </size>";
        }
    }

    public void ChangeResolution(int add)
    {
        indexResolution += add;
        if (indexResolution >= resolutions.Length)
            indexResolution = 0;
        else if (indexResolution < 0)
            indexResolution = resolutions.Length - 1;
        Screen.SetResolution(resolutions[indexResolution].x, resolutions[indexResolution].y, Screen.fullScreenMode);
        resolutionText.text = "<size=150%> <</size>" + resolutions[indexResolution].x + " × " + resolutions[indexResolution].y + "<size=150%>> </size>";
    }

    public void ChangeBrightness()
    {
        brightnessSlider.value = Mathf.Round(brightnessSlider.value * 10) / 10f;
        Screen.brightness = brightnessSlider.value;
        brightessText.text = (brightnessSlider.value * 10).ToString();
    }

    public void DefaultGraphicsSettings()
    {
        qualityText.text = "<size=150%> <</size>" + QualitySettings.names[2] + "<size=150%>> </size>";
        fullscreenModeText.text = "<size=150%> <</size>Full-Screen<size=150%>> </size>";
        indexResolution = resolutions.Length - 1;
        resolutionText.text = "<size=150%> <</size>" + resolutions[indexResolution].x + " × " + resolutions[indexResolution].y + "<size=150%>> </size>";
        brightessText.SetText(Mathf.Round((brightnessSlider.value = Screen.brightness) * 10).ToString());
    }
}
