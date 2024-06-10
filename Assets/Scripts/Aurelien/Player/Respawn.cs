using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    [SerializeField, Header("TimeToDoShader")]
    private float shaderTime = 1f;

    [SerializeField, Header("DeathPrefab")]
    private GameObject corvusDeathEffectSprite;

    [SerializeField] private GameObject luciolVFX, sparkleVFX, impactDeathVFX, blackSplashShader;
    [SerializeField] private SoundData deathSound;

    [SerializeField] private GameObject canvas;

    private Scene currentScene;

    private Vector2 startPosition;

    private Transform meshRenderer;

    private PlayerInput playerInput;

    private SpriteRenderer shaderDeathRespawn;
    private Material blackSplash;

    private float dissolveAmount = 0, verticalDissolve = 1.1f, blackSplashSize = 0;

    private bool spawn, death;

    public static Vector2 spawnPosition;
    public static bool checkPoint;

    private void Awake()
    {
        death = false;

        if (checkPoint) return;
        //GetVisibleObject(false);
        FirstSpawn();
    }

    private void Start()
    {
        spawn = true;
        ControlsParameter.GamePadVibration(this, 0f, 0f, 0f);
        playerInput = PlayerController2D._instance.transform.GetComponent<PlayerInput>();
        playerInput.enabled = false;
        meshRenderer = PlayerController2D._instance.transform.GetComponentInChildren<PlayerAnimation>().transform;
        for (int i = 0; i < meshRenderer.childCount; i++)
            meshRenderer.GetChild(i).gameObject.SetActive(false);

        GameObject blackSplashShaderObj =
            Instantiate(blackSplashShader, transform.position, Quaternion.identity, transform);
        blackSplash = blackSplashShaderObj.GetComponent<SpriteRenderer>().material;
        blackSplashSize = 1f;
        blackSplash.SetFloat("_Size", blackSplashSize);
        DOTween.To(() => blackSplashSize, x => blackSplashSize = x, -1, 6).SetEase(Ease.OutCirc)
            .SetId("blackSplashSize")
            .OnComplete(() => Destroy(blackSplashShaderObj));
        shaderDeathRespawn =
            Instantiate(corvusDeathEffectSprite, PlayerController2D._instance.transform.position, Quaternion.identity,
                PlayerController2D._instance.transform).GetComponent<SpriteRenderer>();
        shaderDeathRespawn.color = Color.red;
        DOTween.To(() => verticalDissolve, x => verticalDissolve = x, 0f, shaderTime)
            .OnComplete(() =>
            {
                spawn = false;
                Destroy(shaderDeathRespawn.gameObject);
                playerInput.enabled = true;
                for (int i = 0; i < meshRenderer.childCount; i++)
                    meshRenderer.GetChild(i).gameObject.SetActive(true);
            //    GetVisibleObject(true);
            });
        if (shaderDeathRespawn == null) return;
        shaderDeathRespawn.material.SetFloat("_VerticalDissolve", 1.1f);
    }

    private void Update()
    {
        if (shaderDeathRespawn == null) return;
        if (spawn)
        {
            shaderDeathRespawn.material.SetFloat("_VerticalDissolve", verticalDissolve);
            if (blackSplash != null)
                blackSplash.SetFloat("_Size", blackSplashSize);
        }
        else if (death)
        {
            shaderDeathRespawn.material.SetFloat("_DissolveAmount", dissolveAmount);
            if (blackSplash != null)
                blackSplash.SetFloat("_Size", blackSplashSize);
        }
    }

    public void RespawnPlayer()
    {
        if (corvusDeathEffectSprite == null)
        {
            currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        else if (death)
        {
            return;
        }

        Death();
    }

    private void Death()
    {
        ControlsParameter.GamePadVibration(this, 0.4f, 0.4f, 0.8f);
        playerInput.enabled = false;
        shaderDeathRespawn =
            Instantiate(corvusDeathEffectSprite, PlayerController2D._instance.transform.position, Quaternion.identity,
                PlayerController2D._instance.transform).GetComponent<SpriteRenderer>();
        Destroy(shaderDeathRespawn.transform.GetChild(0).gameObject);
        shaderDeathRespawn.transform.localScale = new Vector3(
            meshRenderer.GetComponent<PlayerAnimation>().GetDirection
                ? shaderDeathRespawn.transform.localScale.x
                : -shaderDeathRespawn.transform.localScale.x, shaderDeathRespawn.transform.localScale.y,
            shaderDeathRespawn.transform.localScale.z);
        shaderDeathRespawn.material.SetFloat("_DissolveAmount", 0);
        death = true;

        for (int i = 0; i < meshRenderer.childCount; i++)
            meshRenderer.GetChild(i).gameObject.SetActive(false);
        DOTween.Kill("blackSplashSize");
        Sequence sequence = DOTween.Sequence();
        float d = 0;
        GameObject blackSplashShaderObj =
            Instantiate(blackSplashShader, transform.position, Quaternion.identity, transform);
       // GetVisibleObject(false);
        blackSplash = blackSplashShaderObj.GetComponent<SpriteRenderer>().material;
        blackSplashSize = -0.1f;
        blackSplash.SetFloat("_Size", blackSplashSize);
        sequence.Append(DOTween.To(() => d, x => d = x, 1f, 0.2f));
        sequence.AppendCallback(() =>
        {
            AudioManager.instance.PlaySound(deathSound);
            VFXInstantieur.instance.PlayVFXInWorld(impactDeathVFX, transform);
            DOTween.To(() => blackSplashSize, x => blackSplashSize = x, 1f, 6).SetEase(Ease.OutCirc);
        });
        float a = 0;
        sequence.Append(DOTween.To(() => a, x => a = x, 1f, 0.4f));
        sequence.Append(DOTween.To(() => dissolveAmount, x => dissolveAmount = x, 1.1f, shaderTime));
        sequence.AppendCallback(() => VFXInstantieur.instance.PlayVFXInWorld(luciolVFX,
            transform.position + Vector3.down, luciolVFX.transform.localScale, luciolVFX.transform.eulerAngles));
        sequence.AppendCallback(() => VFXInstantieur.instance.PlayVFXInWorld(sparkleVFX, transform));
        float c = 0;
        sequence.Append(DOTween.To(() => c, x => c = x, 1f, 1f));
        sequence.AppendCallback(() =>
        {
            currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        });
    }

    private void FirstSpawn()
    {
        startPosition = transform.position;
        spawnPosition = startPosition;
    }

    private void OnDestroy()
    {
        ControlsParameter.GamePadVibration(this, 0, 0, 0);
    }

    private void GetVisibleObject(bool enable)
    {
        canvas.SetActive(enable);
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer obj in renderers)
        {
            if (obj.isVisible)
            {
                Debug.Log(obj.isVisible);

                obj.gameObject.SetActive(enable);
            }
        }
    }
}