using UnityEngine;
using UnityEngine.Events;

public class BackInMenu : MonoBehaviour
{
    public UnityEvent onBack = new UnityEvent();

    public void Back()
    {
        onBack.Invoke();
    }
}