using UnityEngine;

public class DestroyFragments : MonoBehaviour
{
  private void Start()
  {
    Destroy(gameObject, 5f);
  }
}
