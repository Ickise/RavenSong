using UnityEngine;

public class RaycastDetection : MonoBehaviour
{
        [Header("À set up")]
        [SerializeField] private Transform capsuleCastGround;
        [SerializeField] private Vector2 capsuleSizeGround = new Vector2(1f, 0.1f);

        [SerializeField] private LayerMask layerGround;
        [SerializeField] private LayerMask layerWall;

        [SerializeField] private float distanceToDetectFloor = 0f;
        [SerializeField] private bool groundGizmos;
        private BoxCollider2D cd2D;

        private void Start()
        {
                cd2D = GetComponentInParent<BoxCollider2D>();
        }

        public RaycastHit2D IsGrounded => Physics2D.BoxCast(transform.position + new Vector3(cd2D.offset.x, cd2D.offset.y, 0) + Vector3.down * ((cd2D.size.y + cd2D.edgeRadius * 2f) / 2f), new Vector2(cd2D.size.x + cd2D.edgeRadius * 2f, 0.1f), transform.eulerAngles.z, Vector2.down, 0, layerGround);
        public RaycastHit2D RaycastJump => Physics2D.CapsuleCast(transform.position - capsuleCastGround.localPosition, capsuleSizeGround, CapsuleDirection2D.Horizontal,
                0, Vector2.down, distanceToDetectFloor, layerGround);

        public RaycastHit2D RaycastOnRoll(Vector2 rollDirection)
        {
                return Physics2D.BoxCast(transform.position, new Vector2(0.1f, transform.localScale.y - 0.1f), 0, rollDirection, 1, layerWall | layerGround);
        }

        void OnDrawGizmos()
        {
                if (!groundGizmos) return;
                if (cd2D == null)
                        cd2D = GetComponentInParent<BoxCollider2D>();
                Gizmos.DrawWireCube(transform.position + new Vector3(cd2D.offset.x, cd2D.offset.y, 0) + Vector3.down * ((cd2D.size.y + cd2D.edgeRadius * 2f) / 2f), new Vector2(cd2D.size.x + cd2D.edgeRadius * 2f, 0.1f));
        }
}
