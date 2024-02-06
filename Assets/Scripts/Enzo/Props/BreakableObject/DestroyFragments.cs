using UnityEngine;

public class DestroyFragments : MonoBehaviour
{
    void OnBecameInvisible()
    {
        //lorsque la caméra ne voit plus les objets, cette fonction les détruient
        Destroy(gameObject);
    }
}
