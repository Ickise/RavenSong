using UnityEngine;
using Aurinaxtailer;
using System.Collections;

public class AmmoRebon : Ammo
{
    private bool hasStoped;
    [SerializeField] private LayerMask layerRebond;

    public override void Recover(Gun _gun)
    {
        this._gun = _gun;
        IsRecover = true;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (IsRecover) return;
        if (canPlayerRecover) CanRecover = true;
        if (other.gameObject.layer == LayerMask.NameToLayer("IA"))
        {
            other.GetComponent<IA>().enabled = false;
            Rigidbody2D rbCurrentIA = other.GetComponent<Rigidbody2D>();
            rbCurrentIA.velocity = Vector2.zero;
            rb2D.velocity = Vector2.zero;
            rbCurrentIA.AddForce((transform.position.x > other.transform.position.x ? new Vector2(-1, 1).normalized : new Vector2(1, 1).normalized) * forceRecule, ForceMode2D.Impulse);
            StartCoroutine(EnableIA());
            IEnumerator EnableIA()
            {
                yield return new WaitForSeconds(0.2f);
                other.GetComponent<IA>().enabled = true;
            }
        }
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

    private void FixedUpdate()
    {
        transform.rotation = Rotation2D.LookToDirection2D(transform.rotation, rb2D.velocity);
        Debug.DrawRay(transform.position, transform.up * 1.2f * rb2D.velocity.magnitude * Time.fixedDeltaTime);
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.up, rb2D.velocity.magnitude * Time.fixedDeltaTime, layerRebond);
        if (hit2D)
            OnTriggerEnter2D(hit2D.transform.GetComponent<Collider2D>());
    }
}
