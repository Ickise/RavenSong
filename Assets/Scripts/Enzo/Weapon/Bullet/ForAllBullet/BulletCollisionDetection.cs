using UnityEngine;

public class BulletCollisionDetection : MonoBehaviour
{
    [Header("Ne pas set up")] public RaycastHit2D hitSomething;
    public RaycastHit2D hitGround;

    private Vector2 direction;

    private OnBulletHit onBulletHit;

    [Header("À set up")] [SerializeField] private float distanceToDetect = 0f;
    [SerializeField] private float distanceToDetectGround = 0.12f;

    [SerializeField] private Vector2 capsuleSize;

    [SerializeField] private LayerMask[] listOfLayer;

    [SerializeField] private Transform capsuleCastTransform;

    private void Update()
    {
        SetRaycast();
        InvokeBulletHit();
    }

    private void SetRaycast()
    {
        direction = transform.position.x > 0 ? Vector2.right : Vector2.left;

        hitSomething = Physics2D.CapsuleCast(capsuleCastTransform.position, capsuleSize, CapsuleDirection2D.Horizontal,
            0f,
            direction, distanceToDetect, listOfLayer[0]);

        hitGround = Physics2D.Raycast(transform.position, Vector2.down, distanceToDetectGround, listOfLayer[1]);
    }

    private void InvokeBulletHit()
    {
        if (hitSomething.collider == null)
        {
            return;
        }

        onBulletHit = hitSomething.collider.gameObject.GetComponent<OnBulletHit>();
        onBulletHit.BulletHitSomething();
    }
}