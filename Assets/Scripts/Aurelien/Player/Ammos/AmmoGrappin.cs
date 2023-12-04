using DG.Tweening;
using UnityEngine;

public class AmmoGrappin : Ammo
{
    private SpringJoint2D springJoint2D;
    [SerializeField] private float forceGrappin = 3f;

    public override void Recover()
    {
        IsRecover = true;
    }

    protected override void Trigger(RaycastHit2D hit2D)
    {
        print("oui");
        rb2D.velocity = Vector2.zero;
        Rigidbody2D hit2Drb = hit2D.transform.GetComponent<Rigidbody2D>();
        hit2Drb.velocity = Vector2.zero;
        hit2Drb.bodyType = RigidbodyType2D.Kinematic;
        springJoint2D = _gun.ShootPosition.GetComponent<SpringJoint2D>();

        hit2D.transform.parent = transform;
        springJoint2D.connectedBody = rb2D;
        rb2D.mass += hit2Drb.mass;
        springJoint2D.autoConfigureDistance = false;
        DOTween.To(() => springJoint2D.distance, x => springJoint2D.distance = x, 0f, forceGrappin).SetSpeedBased(true);
    }
}
