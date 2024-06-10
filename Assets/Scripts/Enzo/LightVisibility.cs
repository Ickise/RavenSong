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

        // Calcule le frustum de la caméra
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        // Parcourt chaque lumière et vérifie sa visibilité
        foreach (Light2D light in lights)
        {
            if (IsLightInCameraFrustum2D(light.transform.position, planes))
            {
                Debug.Log("Désactivation de la lumière visible: " + light.name);
                light.transform.parent.gameObject.SetActive(false);
            }
        }
        foreach (IA ia in _ias)
        {
            if (IsLightInCameraFrustum2D(ia.transform.position, planes))
            {
                Debug.Log("Désactivation de la lumière visible: " + ia.name);
                ia.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private bool IsLightInCameraFrustum2D(Vector3 position, Plane[] planes)
    {
        // Utilise GeometryUtility pour vérifier si la position est dans le frustum
        return GeometryUtility.TestPlanesAABB(planes, new Bounds(position, Vector3.zero));
    }
}