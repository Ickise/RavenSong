using UnityEngine;

public class AmmoGravity : Ammo
{
    [SerializeField] private GameObject noGravityZone, noGravityTrail;
    [SerializeField] private bool trail;
    private GameObject currentNoGravityZone;

    protected override void Awake()
    {
        base.Awake();
        if (trail)
        {
            gameObject.layer = LayerMask.NameToLayer("AmmoDontCollideProps");
            currentNoGravityZone = Instantiate(noGravityTrail, Vector2.zero, Quaternion.identity);
            GetComponent<TrailRenderer>().minVertexDistance = 3f;
        }
    }

    protected override void Trigger(RaycastHit2D hit2D)
    {
        if (canPlayerRecover) CanRecover = true;
        if (trail) return;
        gameObject.SetActive(false);
        GetComponent<TrailRenderer>().minVertexDistance = 0.1f;
        currentNoGravityZone = Instantiate(noGravityZone, transform.position, Quaternion.identity);
    }

    public override void Recover()
    {
        gameObject.SetActive(true);
        Destroy(currentNoGravityZone);
        IsRecover = true;
    }
}
