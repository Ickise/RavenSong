using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    [SerializeField, Header("TimeToDoShader")] private float shaderTime = 1f;
    
    [SerializeField, Header("DeathPrefab")] private GameObject corvusDeathEffectSprite;
    
    private Scene currentScene;
    
    private Vector2 startPosition;

    private Transform meshRenderer;
    
    private PlayerInput playerInput;
    
    private SpriteRenderer shaderDeathRespawn;
    
    private float dissolveAmount = 0, verticalDissolve = 1.1f;
    
    private bool spawn, death;
    
    public static Vector2 spawnPosition;
    public static bool checkPoint;

    private void Awake()
    {
        if (checkPoint) return;
        
        FirstSpawn();
    }

    private void Start()
    {
        spawn = true;
        playerInput = PlayerController2D._instance.transform.GetComponent<PlayerInput>();
        playerInput.enabled = false;
        meshRenderer = PlayerController2D._instance.transform.GetComponentInChildren<PlayerAnimation>().transform;
        for (int i = 0; i < meshRenderer.childCount; i++)
            meshRenderer.GetChild(i).gameObject.SetActive(false);

        shaderDeathRespawn =
            Instantiate(corvusDeathEffectSprite, PlayerController2D._instance.transform.position, Quaternion.identity,
                PlayerController2D._instance.transform).GetComponent<SpriteRenderer>();
        DOTween.To(() => verticalDissolve, x => verticalDissolve = x, 0f, shaderTime)
            .OnComplete(() =>
            {
                spawn = false;
                Destroy(shaderDeathRespawn.gameObject);
                playerInput.enabled = true;
                for (int i = 0; i < meshRenderer.childCount; i++)
                    meshRenderer.GetChild(i).gameObject.SetActive(true);
            });
        if (shaderDeathRespawn == null) return;
        shaderDeathRespawn.material.SetFloat("_VerticalDissolve", 1.1f);
    }

    private void Update()
    {
        if (shaderDeathRespawn == null) return;
        if (spawn)
            shaderDeathRespawn.material.SetFloat("_VerticalDissolve", verticalDissolve);
        else if (death)
            shaderDeathRespawn.material.SetFloat("_DissolveAmount", dissolveAmount);
    }

    public void RespawnPlayer()
    {
        if (corvusDeathEffectSprite == null)
        {
            currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        else if (death) return;

        Death();
    }

    private void Death()
    {
        playerInput.enabled = false;
        shaderDeathRespawn =
            Instantiate(corvusDeathEffectSprite, PlayerController2D._instance.transform.position, Quaternion.identity,
                PlayerController2D._instance.transform).GetComponent<SpriteRenderer>();
        shaderDeathRespawn.transform.localScale = new Vector3(
            meshRenderer.GetComponent<PlayerAnimation>().GetDirection
                ? shaderDeathRespawn.transform.localScale.x
                : -shaderDeathRespawn.transform.localScale.x, shaderDeathRespawn.transform.localScale.y,
            shaderDeathRespawn.transform.localScale.z);
        shaderDeathRespawn.material.SetFloat("_DissolveAmount", 0);
        death = true;
        for (int i = 0; i < meshRenderer.childCount; i++)
            meshRenderer.GetChild(i).gameObject.SetActive(false);
        DOTween.To(() => dissolveAmount, x => dissolveAmount = x, 1.1f, shaderTime)
            .OnComplete(() =>
            {
                currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
                death = false;
            });
    }

    private void FirstSpawn()
    {
        startPosition = transform.position;
        spawnPosition = startPosition;
    }
}