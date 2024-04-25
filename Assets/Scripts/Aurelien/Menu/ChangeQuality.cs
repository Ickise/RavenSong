using TMPro;
using UnityEngine;

public class ChangeQuality : MonoBehaviour
{
    private TextMeshProUGUI qualityText;
    private int indexQuality;
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

    private void Start()
    {
        qualityText = GetComponent<TextMeshProUGUI>();
        qualityText.text = QualitySettings.names[indexQuality = QualitySettings.GetQualityLevel()];
    }
}
