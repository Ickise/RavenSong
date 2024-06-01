using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputReader : MonoBehaviour
{
    private BackInMenu activeBackInMenu;
    [SerializeField] private SoundData back, selection;

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AudioManager.instance.PlaySound(back);
            activeBackInMenu = gameObject.GetComponentInChildren<BackInMenu>();
            activeBackInMenu.Back();
        }
    }

    public void VerticalNavigation(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        AudioManager.instance.PlaySound(selection);
    }
}
