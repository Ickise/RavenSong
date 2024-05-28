    using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MenuInputReader : MonoBehaviour
{
    public static MenuInputReader _instance;

    public UnityEvent onPause = new UnityEvent();

    private void Awake()
    {
        _instance = this;
    }
    
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.LogError("no");

            onPause.Invoke();
        }
    }
}
