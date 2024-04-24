using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private Scene currentScene;
    [SerializeField] private Vector2 startPosition = new Vector2(7.5f, 3);
    [SerializeField] private float shaderTime = 1f;
    [SerializeField] private GameObject corvusDeathEffectSprite;
    private PlayerInput playerInput;
    private SpriteRenderer shaderDeathRespawn;
    public static Vector2 spawnPosition;
    public static bool doResetSpawn;
    private float dissolveAmount = 0, verticalDissolve = 1.1f;
    private bool spawn, death;

    private void Awake()
    {
        if (doResetSpawn)
        {
            spawnPosition = startPosition;
            doResetSpawn = false;
        }
    }

    private void Start()
    {
        spawn = true;
        playerInput = PlayerController2D._instance.transform.GetComponent<PlayerInput>();
        playerInput.enabled = false;
        shaderDeathRespawn = Instantiate(corvusDeathEffectSprite, PlayerController2D._instance.transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
        DOTween.To(() => verticalDissolve, x => verticalDissolve = x, 0f, shaderTime)
        .OnComplete(() => { spawn = false; Destroy(shaderDeathRespawn); });
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
        if (shaderDeathRespawn == null)
        {
            currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        else if (death) return;
        Death();
    }

    private void Death()
    {
        shaderDeathRespawn.material.SetFloat("_DissolveAmount", 0);
        death = true;
        shaderDeathRespawn = Instantiate(corvusDeathEffectSprite, PlayerController2D._instance.transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
        DOTween.To(() => dissolveAmount, x => dissolveAmount = x, 1.1f, shaderTime)
        .OnComplete(() =>
        {
            currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            death = false;
        });
    }
}
