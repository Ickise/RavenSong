using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioParameter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] volumeTexts;

    private void Start()
    {
        Slider[] sliders = GetComponentsInChildren<Slider>();
        for (int i = 0; i < sliders.Length; i++)
            volumeTexts[i].SetText(((sliders[i].value = AudioManager.volumeScale) * 10).ToString());
    }

    public void ChangeGlobalVolume(TextMeshProUGUI text)
    {
        Slider slider = text.transform.parent.GetComponentInChildren<Slider>();
        slider.value = Mathf.Round(slider.value * 10) / 10f;
        AudioManager.volumeScale = slider.value;
        text.text = Mathf.Round(slider.value * 10).ToString();
    }
}
