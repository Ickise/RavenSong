using UnityEngine;

public class InvisibleDissable : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
    
    private void OnBecameVisible()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }
}
