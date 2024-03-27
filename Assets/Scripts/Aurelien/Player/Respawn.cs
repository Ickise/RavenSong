using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private Scene currentScene;
    [SerializeField] private Vector2 startPosition = new Vector2(7.5f, 3);
    public static Vector2 spawnPosition;

    private void Awake()
    {
        if (Time.realtimeSinceStartup < 8)
            spawnPosition = startPosition;  
    }

    public void RespawnPlayer()
    {
        currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
