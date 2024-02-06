using UnityEngine;
using UnityEngine.Events;

public class OnBulletHit : MonoBehaviour
{
    public UnityEvent onBulletHit = new UnityEvent();

    public void BulletHitSomething()
    {
        onBulletHit.Invoke();
    }
}