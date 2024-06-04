using UnityEngine;
using UnityEngine.Events;

public class OnCrossKickHit : MonoBehaviour
{
    public UnityEvent onCrossKickHit = new UnityEvent();

    public void OnHIt()
    {
        onCrossKickHit.Invoke();
    }
}