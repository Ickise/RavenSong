using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputReader : MonoBehaviour
{
    [SerializeField] private SoundData back, selection;

    private BackInMenu activeBackInMenu;

    private DefaultValueInMenu _defaultValue;

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            activeBackInMenu = gameObject.GetComponentInChildren<BackInMenu>();
            if (activeBackInMenu == null) return;
            AudioManager.instance.PlaySound(back);
            activeBackInMenu.Back();
        }
    }

    public void VerticalNavigation(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        AudioManager.instance.PlaySound(selection);
    }

    public void OnDefault(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _defaultValue = gameObject.GetComponentInChildren<DefaultValueInMenu>();
            if (_defaultValue == null) return;
            AudioManager.instance.PlaySound(selection);
            _defaultValue.DefaultSettings();
        }
    }
}