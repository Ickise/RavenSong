using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsParameter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI qualityText, fullscreenModeText, resolutionText, brightessText;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Vector2Int[] resolutions;
    private int indexQuality, indexFullscreenMode, indexResolution;

    private void Start()
    {
        qualityText.text = QualitySettings.names[indexQuality = QualitySettings.GetQualityLevel()];
        fullscreenModeText.text = "plein écran";
        indexResolution = resolutions.Length - 1;
        resolutionText.text = resolutions[indexResolution].x + " × " + resolutions[indexResolution].y;
        brightessText.SetText(((brightnessSlider.value = Screen.brightness) * 10).ToString());
    }
    public void ChangeQuality(int add)
    {
        indexQuality += add;
        if (indexQuality >= QualitySettings.count)
            indexQuality = 0;
        else if (indexQuality < 0)
            indexQuality = QualitySettings.count - 1;
        QualitySettings.SetQualityLevel(indexQuality, true);
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
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
            fullscreenModeText.text = "plein écran";
        }
        else if (indexFullscreenMode == 1)
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.MaximizedWindow);
            fullscreenModeText.text = "fenêtre sans bordure";
        }
        else if (indexFullscreenMode == 2)
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
            fullscreenModeText.text = "fenêtre";
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
        resolutionText.text = resolutions[indexResolution].x + " × " + resolutions[indexResolution].y;
    }

    public void ChangeBrightness()
    {
        brightnessSlider.value = Mathf.Round(brightnessSlider.value * 10) / 10f;
        Screen.brightness = brightnessSlider.value;
        brightessText.text = (brightnessSlider.value * 10).ToString();
    }
}
