using UnityEngine;
using UnityEngine.Events;

public class DefaultValueInMenu : MonoBehaviour
{
    public UnityEvent onDefaultSettings = new UnityEvent();

    public void DefaultSettings()
    {
        onDefaultSettings.Invoke();
    }
}
