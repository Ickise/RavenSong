using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyCorpse : MonoBehaviour
{
    private Collider2D corpseCollider;

    private Rigidbody2D corpseRigidbody;

    [SerializeField, Header("Layer"), Tooltip("Les layers que nous utilisons pour le sol ")]
    private LayerMask layerGround;

    public static bool bulletInCorpse;

    [SerializeField, Header("Float"), Tooltip("Distance pour détecter le sol")] private float distance;

    [SerializeField, Header("Time to destroy components"), Range(0.1f, 0.5f)]
    private float timeToDestroyComponents = 0.3f;

    private void Awake()
    {
        GetComponents();

        bulletInCorpse = true;
    }

    private void Update()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, distance, layerGround);
        Debug.Log(hit2D.collider.name);

        if (hit2D.collider != null)
        {
            ChangeLayer();
            DestroyComponents();
        }
    }

    private void GetComponents()
    {
        corpseCollider = GetComponent<Collider2D>();

        corpseRigidbody = GetComponent<Rigidbody2D>();
    }

    private void DestroyComponents()
    {
        Object[] listToDestroy = { corpseCollider, corpseRigidbody, this };

        foreach (var elementToDestroy in listToDestroy)
        {
            Destroy(elementToDestroy);
        }
    }

    private void ChangeLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("IADontCollide");
    }
}