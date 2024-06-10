using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightVisibility : MonoBehaviour
{
    private Camera mainCamera;

    private Light2D[] lights;
    private IA[] _ias;


    void Start()
    {
        lights = FindObjectsOfType<Light2D>();
        _ias = FindObjectsOfType<IA>();
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    public void DisableVisibleLights2D()
    {
        if (mainCamera == null) return;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        foreach (Light2D light in lights)
        {
            if (IsLightInCameraFrustum2D(light.transform.position, planes))
            {
                light.transform.parent.gameObject.SetActive(false);
            }
        }
        foreach (IA ia in _ias)
        {
            if (IsLightInCameraFrustum2D(ia.transform.position, planes))
            {
                ia.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private bool IsLightInCameraFrustum2D(Vector3 position, Plane[] planes)
    {
        return GeometryUtility.TestPlanesAABB(planes, new Bounds(position, Vector3.zero));
    }
}