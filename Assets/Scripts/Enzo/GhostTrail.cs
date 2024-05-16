using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    [SerializeField, Header("Prefab")] private GameObject ghostTrail;

    [SerializeField, Header("Float"), Range(0.1f, 1f)]
    private float delay = 0.2f;

    [SerializeField, Range(0.1f, 1f)] private float destroyTime = 0.1f;

    [SerializeField, Header("Player Mesh")]
    private MeshFilter rightMeshFilter;

    [SerializeField] private MeshFilter leftMeshFilter;

    private float delta;

    private PlayerAnimation _playerAnimation;

    private MeshRenderer meshRenderer;

    private MeshFilter meshFilter;

    void Start()
    {
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    private void OnDisable()
    {
        delta = 0;
    }

    private void Update()
    {
        delta += 0.02f;

        if (delta >= delay)
        {
            CreateGhost();
            delta = 0;
        }
    }

    private void CreateGhost()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y - 1);
        GameObject ghostObj = Instantiate(ghostTrail, position, transform.rotation);
        ghostObj.transform.localScale = PlayerController2D._instance.transform.localScale;
        Destroy(ghostObj, destroyTime);

        meshFilter = ghostObj.GetComponent<MeshFilter>();
        meshFilter.mesh = _playerAnimation.GetDirection
            ? rightMeshFilter.mesh
            : leftMeshFilter.mesh;

        meshRenderer = ghostObj.GetComponent<MeshRenderer>();
        meshRenderer.materials = _playerAnimation.GetDirection
            ? rightMeshFilter.GetComponent<MeshRenderer>().materials
            : leftMeshFilter.GetComponent<MeshRenderer>().materials;
    }
}