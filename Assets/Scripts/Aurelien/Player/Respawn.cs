using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private Scene currentScene;
    [SerializeField] private Vector2 startPosition = new Vector2(7.5f, 3);
    public static Vector2 spawnPosition;
    private bool resetSpawn = true;

    public void RespawnPlayer()
    {
        currentScene = SceneManager.GetActiveScene();
        resetSpawn = false;
        SceneManager.LoadScene(currentScene.name);
    }

    private void OnDisable()
    {
        if (resetSpawn)
            spawnPosition = startPosition;
    }
}
