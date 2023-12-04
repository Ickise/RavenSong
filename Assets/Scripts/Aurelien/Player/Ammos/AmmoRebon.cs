using UnityEngine;
using Aurinaxtailer;
using System.Collections;
using DG.Tweening;

public class AmmoRebon : Ammo
{
    private bool hasStoped;

    public override void Recover()
    {
        IsRecover = true;
    }

    protected override void Trigger(RaycastHit2D hit2D)
    {
        active = true;
        if (IsRecover) return;
        if (canPlayerRecover) CanRecover = true;
        if (hit2D.transform.gameObject.layer == LayerMask.NameToLayer("IA"))
        {
            IA currentIA = hit2D.transform.GetComponent<IA>();
            currentIA.enabled = false;
            Rigidbody2D rbCurrentIA = hit2D.transform.GetComponent<Rigidbody2D>();
            rbCurrentIA.velocity = Vector2.zero;
            // rb2D.velocity = Vector2.zero;
            rbCurrentIA.AddForce((transform.position.x > currentIA.transform.position.x ? new Vector2(-1, 1).normalized : new Vector2(1, 1).normalized) * forceRecule, ForceMode2D.Impulse);
            currentIA.NbVie -= 1;
            if (currentIA.NbVie < 1)
            {
                rbCurrentIA.constraints = RigidbodyConstraints2D.None;
                rbCurrentIA.DORotate(transform.position.x > currentIA.transform.position.x ? -90f : 90f, 0.2f)
                .OnComplete(() => { Destroy(currentIA.gameObject); StopAllCoroutines(); });
            }
            StartCoroutine(EnableIA());
            IEnumerator EnableIA()
            {
                yield return new WaitForSeconds(0.2f);
                currentIA.enabled = true;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!hasStoped && ammoMagnitude < 100)
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
