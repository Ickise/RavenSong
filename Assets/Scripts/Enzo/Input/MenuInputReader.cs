using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputReader : MonoBehaviour
{
    private Transform activBackGameObject;

    private BackInMenu activBack;

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            activBack = gameObject.GetComponentInChildren<BackInMenu>();
            activBack.Back();
            Debug.Log(activBack.name);
        }
    }
}
