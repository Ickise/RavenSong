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
    public void PlayVFXInWorld(GameObject vfxGameobject, Transform position, float timeDestroy = 3)
    {
        if (vfxGameobject == null)
        {
            Debug.LogWarning("le vfx sur " + vfxGameobject.gameObject.name + " n'est pas set");
            return;
        }
        GameObject currentVFX = Instantiate(vfxGameobject, position.position, position.rotation);
        Destroy(currentVFX, timeDestroy);
    }

    public void PlayVFXInWorld(GameObject vfxGameobject, Vector3 position, Vector3 localScale, Quaternion rotation, float timeDestroy = 3)
    {
        if (vfxGameobject == null)
        {
            Debug.LogWarning("le vfx sur " + vfxGameobject.gameObject.name + " n'est pas set");
            return;
        }
        GameObject currentVFX = Instantiate(vfxGameobject, position, rotation);
        Destroy(currentVFX, timeDestroy);
    }
}
