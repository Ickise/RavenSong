using UnityEngine;
using DG.Tweening;
using Aurinaxtailer;

public class AmmoTest : Ammo
{
    private GameObject electricCollider;

    protected override void Trigger(GameObject other)
    {
        if (IsRecover) return;
        if (canPlayerRecover) CanRecover = true;
        rb2D.velocity = Vector2.zero;
        if (other.CompareTag("Electric React"))
        {
            SpriteRenderer otherSprite = other.GetComponent<SpriteRenderer>();
            otherSprite.color = Color.red;
            otherSprite.DOColor(Color.white, 1f);
            electricCollider = new GameObject();
            CopyComponent.CopyTransform(other.transform, electricCollider.transform, true);
            electricCollider.transform.localScale *= 1.01f;
            electricCollider.AddComponent<BoxCollider2D>().isTrigger = true;
            electricCollider.AddComponent<ElectricEffectOnObject>();
            Destroy(electricCollider, 0.5f);
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("IA"))
        {
            other.transform.DOShakePosition(3f, Vector3.right * 0.1f, 30, 0, false, false, ShakeRandomnessMode.Full);
            gameObject.SetActive(false);
        }
    }

    public override void Recover()
    {
        gameObject.SetActive(true);
        IsRecover = true;
    }
}
