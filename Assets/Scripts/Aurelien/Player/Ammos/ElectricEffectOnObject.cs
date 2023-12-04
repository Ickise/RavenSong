using UnityEngine;
using DG.Tweening;

public class ElectricEffectOnObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("IA") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
            other.transform.DOShakePosition(3f, Vector3.right * 0.1f, 30, 0, false, false, ShakeRandomnessMode.Full);
    }
}
