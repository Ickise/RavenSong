using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputReader : MonoBehaviour
{
    private BackInMenu activeBackInMenu;

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            activeBackInMenu = gameObject.GetComponentInChildren<BackInMenu>();
            activeBackInMenu.Back();
        }
    }
}
