using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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

    public void ManetteControl(InputAction.CallbackContext context)
    {
        if (!context.started || !gameObject.activeInHierarchy) return;
        int add = Mathf.RoundToInt(Mathf.Clamp(context.ReadValue<float>() * Mathf.Infinity, -1, 1));
        Slider slider = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Slider>();
        if (slider != null)
        {
            slider.value = Mathf.Round((slider.value * 10) + add) / 10f;
            AudioManager.volumeScale = slider.value;
            TextMeshProUGUI[] text = slider.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
            text[text.Length - 1].text = Mathf.Round(slider.value * 10).ToString();
        }
    }

    public void ChangeGlobalVolume(TextMeshProUGUI text)
    {
        Slider slider = text.transform.parent.GetComponentInChildren<Slider>();
        slider.value = Mathf.Round(slider.value * 10) / 10f;
        AudioManager.volumeScale = slider.value;
        text.text = Mathf.Round(slider.value * 10).ToString();
    }
}
