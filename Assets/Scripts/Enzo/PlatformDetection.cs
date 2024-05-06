using UnityEngine;

public class PlatformDetection : MonoBehaviour
{
    private GameObject currentPlatform;

    private Collider2D currentPlatformCollider;
    [SerializeField] private GameObject vfxDescentPlatform;

    [SerializeField, Header("BoxCast Size")]
    private Vector2 boxSize = new Vector2(1f, 0.1f);
    
    [SerializeField, Header("Platform Scale Y"), Range(0.1f, 1f),
     Tooltip(
         "Il faut mettre la même valeur que celle en Y du scale des platformes. Il faut que le scale en Y soit le même pour toutes les plateformes !!")]
    private float platformScaleY = 0.5f;

    private void FixedUpdate()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(transform.position + Vector3.down * platformScaleY, boxSize, 0,
            Vector3.down * Time.fixedDeltaTime,
            PlayerController2D._instance.PlayerVelocity.magnitude * Time.fixedDeltaTime);

        if (hit2D.collider != null && hit2D.collider.CompareTag("Platforme"))
        {
            SetPlatformAsGround(hit2D);
        }
        else if (currentPlatform != null)
        {
            SetPlatformAsPlatform();
        }

        if (currentPlatform != null && InputReader.instance.canJump && InputReader.instance.canDown)
        {
            SetPlatformAsPlatform();
            VFXInstantieur.instance.PlayVFXInWorld(vfxDescentPlatform, transform);
        }
    }

    private void SetPlatformAsGround(RaycastHit2D raycastHit2D)
    {
        currentPlatform = raycastHit2D.collider.gameObject;
        currentPlatformCollider = raycastHit2D.collider;
        ChangeTriggerAndLayer(false, "Ground");
    }

    private void SetPlatformAsPlatform()
    {
        ChangeTriggerAndLayer(true, "Escalier");
        currentPlatform = null;
    }

    private void ChangeTriggerAndLayer(bool isTrigger, string nameOfLayer)
    {
        currentPlatformCollider.isTrigger = isTrigger;
        currentPlatform.layer = LayerMask.NameToLayer(nameOfLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + Vector3.down * platformScaleY, boxSize);

        /* Debug.DrawRay(
             transform.position + new Vector3(rb2D.velocity.normalized.x, rb2D.velocity.normalized.y, 0) *
             Time.fixedDeltaTime, rb2D.velocity);*/
    }
}