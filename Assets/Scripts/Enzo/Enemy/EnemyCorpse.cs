using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
  [SerializeField] private GameObject bullet;

  
  private void Update()
  {
    bullet.transform.position = transform.position;
  }
}
