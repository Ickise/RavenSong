using System.Collections;
using DG.Tweening;
using UnityEngine;

public class AmmoDefault : Ammo
{
    public override void Recover()
    {
        IsRecover = true;
    }
    protected override void Trigger(RaycastHit2D hit2D)
    {
        if (canPlayerRecover) CanRecover = true;
        if (hit2D.transform.gameObject.layer == LayerMask.NameToLayer("IA"))
        {
            IA currentIA = hit2D.transform.GetComponent<IA>();
            currentIA.enabled = false;
            Rigidbody2D rbCurrentIA = hit2D.transform.GetComponent<Rigidbody2D>();
            rbCurrentIA.velocity = Vector2.zero;
            rb2D.velocity = Vector2.zero;
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
}
