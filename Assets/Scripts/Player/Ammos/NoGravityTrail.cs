using System.Collections.Generic;
using UnityEngine;

public class NoGravityTrail : NoGravityZone
{
    private List<Vector2> points = new List<Vector2>();
    private EdgeCollider2D edgeCollider2D;
    private Transform noGravityAmmoPosition;

    protected void Start()
    {
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        noGravityAmmoPosition = GameObject.FindGameObjectWithTag("NoGravityAmmo").transform;
        points.Add(noGravityAmmoPosition.position);
        edgeCollider2D.SetPoints(points);
    }

    private void Update()
    {
        if (!noGravityAmmoPosition.gameObject.activeInHierarchy) return;
        points.Add(noGravityAmmoPosition.position);
        edgeCollider2D.SetPoints(points);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    protected override void OnTriggerExit2D(Collider2D other) { }

    private void OnDisable()
    {
        foreach (KeyValuePair<Rigidbody2D, float> item in elementNoGravity)
            item.Key.gravityScale = item.Value;
    }
}
