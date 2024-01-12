using Unity.VisualScripting;
using UnityEngine;

public class AmmoGrappin : Ammo
{
    private SpringJoint2D springJoint2D;
    private bool attached;
    [SerializeField] private bool grappinBreak;
    private LineRenderer lineRenderer;
    [SerializeField] private LayerMask layerMaskBreak;

    protected override void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Recover()
    {
        IsRecover = true;
    }

    protected override void Trigger(RaycastHit2D hit2D)
    {
        CanRecover = true;
        if (hit2D.transform.GetComponent<Rigidbody2D>() == null) return;
        attached = true;
        rb2D.velocity = Vector2.zero;
        c2D.enabled = false;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        transform.parent = hit2D.transform;
        Rigidbody2D hit2Drb = hit2D.transform.GetComponent<Rigidbody2D>();
        hit2Drb.velocity = Vector2.zero;
        springJoint2D = _gun.GetComponentInParent<SpringJoint2D>();
        springJoint2D.enabled = true;
        springJoint2D.connectedBody = hit2Drb;
        springJoint2D.connectedAnchor = transform.localPosition;
    }

    protected override void Update()
    {
        lineRenderer.SetPosition(0, _gun.ShootPosition.position);
        lineRenderer.SetPosition(1, transform.position);
        if (IsRecover)
        {
            attached = false;
            transform.parent = null;
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            c2D.enabled = true;
            if (springJoint2D != null)
                springJoint2D.enabled = false;
            if (!canPlayerRecover)
                lineRenderer.enabled = false;
        }
        if (canPlayerRecover)
            base.Update();
        else if (Vector2.Distance(_gun.transform.position, transform.position) < 1.3f)
            Destroy(gameObject);
        if (!attached) return;
        if (grappinBreak && Physics2D.Raycast(_gun.ShootPosition.position, transform.position - _gun.ShootPosition.position, Vector3.Distance(_gun.ShootPosition.position, transform.position), layerMaskBreak))
            IsRecover = true;
        springJoint2D.anchor = _gun.transform.parent.InverseTransformPoint(_gun.ShootPosition.position);
    }

    private void OnDestroy()
    {
        if (!attached) return;
        attached = false;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        c2D.enabled = true;
        if (springJoint2D != null)
            springJoint2D.enabled = false;
    }
}
