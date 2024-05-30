using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SetFirstButton : MonoBehaviour
{
    private EventSystem eventSystem;

    [SerializeField] private GameObject firstSelectedGameObject;

    public UnityEvent onEnableEvent = new UnityEvent();
    public UnityEvent onDisableEvent = new UnityEvent();

    private void Awake()
    {
        eventSystem = EventSystem.current;
    }
    private void OnEnable()
    {
        onEnableEvent.Invoke();
    }

    private void OnDisable()
    {
        onDisableEvent.Invoke();
    }

    public void SetFirstSelectedButton()
    {
        eventSystem.SetSelectedGameObject(firstSelectedGameObject);
    }
}
