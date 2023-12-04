using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    public Vector3 CurrentRespawnPoint { get; set; }

    private void Awake()
    {
        CurrentRespawnPoint = spawnPoint.position;
    }

    public void RespawnPlayer()
    {
        transform.position = CurrentRespawnPoint;
    }
}
