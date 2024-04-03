using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private Scene currentScene;
    [SerializeField] private Vector2 startPosition = new Vector2(7.5f, 3);
    [SerializeField] private SpriteRenderer shaderDeathRespawn;
    [SerializeField] private float shaderTime = 1f, timeStartup = 8;
    public static Vector2 spawnPosition;
    private float dissolveAmount = 0, verticalDissolve = 1.1f;
    private bool spawn, death;

    private void Awake()
    {
        if (Time.realtimeSinceStartup < timeStartup)
            spawnPosition = startPosition;
    }

    private void Start()
    {
        shaderDeathRespawn.material.SetFloat("_VerticalDissolve", 1.1f);
        spawn = true;
        DOTween.To(() => verticalDissolve, x => verticalDissolve = x, 0f, shaderTime)
        .OnComplete(() => spawn = false);
    }

    private void Update()
    {
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
        DOTween.To(() => dissolveAmount, x => dissolveAmount = x, 1.1f, shaderTime)
        .OnComplete(() =>
        {
            currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            death = false;
        });
    }
}
