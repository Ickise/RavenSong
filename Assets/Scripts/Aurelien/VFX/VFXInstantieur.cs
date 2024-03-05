using UnityEngine;

public class VFXInstantieur : MonoBehaviour
{
    public static VFXInstantieur instance;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Instantie un VFX dans le world et le détruit après un certain temps
    /// </summary>
    /// <param name="vfxGameobject"></param>
    /// <param name="position"></param>
    /// <param name="timeDestroy"></param>
    public void PlayerVFXInWorld(GameObject vfxGameobject, Transform position, float timeDestroy)
    {
        GameObject currentVFX = Instantiate(vfxGameobject, position.position, Quaternion.identity);
        currentVFX.transform.localScale = position.localScale;
        Destroy(currentVFX, timeDestroy);
    }
}
