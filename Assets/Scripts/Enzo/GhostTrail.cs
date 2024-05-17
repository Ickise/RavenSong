using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    [SerializeField, Header("Prefab")] private GameObject ghostTrail;

    [SerializeField, Header("Alpha"), Range(0f, 1f)]
    private float alpha = 0.1f;

    [SerializeField, Header("Float"), Range(0.1f, 1f)]
    private float delay = 0.2f;

    [SerializeField, Range(0.1f, 1f)] private float destroyTime = 0.1f;

    [SerializeField, Header("Player Mesh")]
    private MeshFilter rightMeshFilter;

    [SerializeField] private MeshFilter leftMeshFilter;

    private float delta;
    private float timer = 0.1f;

    private PlayerAnimation _playerAnimation;

    private MeshRenderer meshRenderer;

    private MeshFilter meshFilter;

    void Start()
    {
        timer = 0.1f;
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    private void OnDisable()
    {
        ResetVariables();
    }

    private void Update()
    {
        if (timer >= PlayerController2D._instance.rouladeTime) return;

        LaunchTimer();
        CreateGhost();
    }

    private void CreateGhost()
    {
        if (delta >= delay)
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

            foreach (Material material in meshRenderer.materials)
            {
                material.SetFloat("_alpha", alpha);
            }

            delta = 0;
        }
    }

    private void LaunchTimer()
    {
        timer += Time.deltaTime;

        delta += delay / 2;
    }

    private void ResetVariables()
    {
        delta = 0;
        timer = 0.1f;
    }
}