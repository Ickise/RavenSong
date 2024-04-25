using TMPro;
using UnityEngine;

public class GraphicsParameter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI qualityText, fullscreenModeText;
    private int indexQuality, indexFullscreenMode;

    private void Start()
    {
        qualityText.text = QualitySettings.names[indexQuality = QualitySettings.GetQualityLevel()];
    }
    public void QualityChange(int add)
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
}
