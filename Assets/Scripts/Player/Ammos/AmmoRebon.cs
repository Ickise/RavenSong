using UnityEngine;
using DG.Tweening;

public class AmmoRebon : Ammo
{
    private bool hasStoped;

    public override void Recover(Gun _gun)
    {
        this._gun = _gun;
        IsRecover = true;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (IsRecover) return;
        CanRecover = true;
    }

    protected override void Update()
    {
        base.Update();
        if (!hasStoped && rb2D.velocity.magnitude < 100)
        {
            // rb2D.velocity = Vector2.zero;
            // DOTween.To(() => rb2D.velocity, x => rb2D.velocity = x, Vector2.zero, 3f);
            // rb2D.drag = 1f;
            rb2D.gravityScale = 1f;
            rb2D.sharedMaterial = null;
            hasStoped = true;
        }
    }
}
