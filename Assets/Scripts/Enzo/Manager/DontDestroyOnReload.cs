using UnityEngine;

public class DontDestroyOnReload : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
