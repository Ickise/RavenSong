using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    [SerializeField] private string sceneName;
    
    public void RespawnPlayer()
    {
        SceneManager.LoadScene(sceneName);
    }
}
