using UnityEngine;
using Aurinaxtailer;
using System.Collections.Generic;
using UnityEngine.InputSystem.Interactions;

public class BallInterfacePosition : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    private Camera cam;
    private Vector3 DLWP, ULWP, DRWP, URWP;
    private List<Vector3> screenAngles = new List<Vector3>();
    private List<Vector2> intersectionAmmos = new List<Vector2>();

    void Start()
    {
        cam = Camera.main;
        SetViewportToWorldPoints();
    }

    void Update()
    {
        SetViewportToWorldPoints();
        intersectionAmmos = new List<Vector2>();
        foreach (var ammo in _gun.CurrentAmmos)
        {
            if (ammo == null) return;
            if (!IsOnScreen(ammo.transform.position))
                intersectionAmmos.Add(CalculeIntersectionSegments(ammo.transform.position));
            //     Indicator(ammo);
        }
    }

    private void Indicator(Ammo ammo)
    {
        // print(Camera.main.WorldToViewportPoint(ammo.transform.position));
    }

    private void SetViewportToWorldPoints()
    {
        DLWP = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        ULWP = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));
        URWP = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        DRWP = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        screenAngles = new List<Vector3>() { DLWP, ULWP, URWP, DRWP, DLWP };
    }

    private Vector2 CalculeIntersectionSegments(Vector2 currentAmmoPos)
    {
        List<Vector2> intersections = new List<Vector2>();
        Vector2 gunPos = _gun.transform.position;
        for (int i = 0; i < screenAngles.Count - 1; i++)
            intersections.Add(Intersection2D.GetIntersectionBetweenABandCD(screenAngles[i], screenAngles[i + 1], gunPos, currentAmmoPos));

        List<Vector2> interOnScreen = new List<Vector2>();
        foreach (var intersection in intersections)
        {
            // print(intersection);
            if (IsOnScreen(intersection))
            {
                interOnScreen.Add(intersection);
                // print("oui");
            }
        }

        print(interOnScreen.Count);
        return Vector2.Distance(interOnScreen[0], currentAmmoPos) < Vector2.Distance(interOnScreen[1], currentAmmoPos) ? interOnScreen[0] : interOnScreen[1];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(DLWP, 1F);
        Gizmos.DrawWireSphere(ULWP, 1F);
        Gizmos.DrawWireSphere(URWP, 1F);
        Gizmos.DrawWireSphere(DRWP, 1F);
        Gizmos.color = Color.green;
        foreach (var intersectionAmmo in intersectionAmmos)
            Gizmos.DrawWireSphere(intersectionAmmo, 3F);
    }

    private bool IsOnScreen(Vector3 ammoPos)
    {
        // print(ULWP + "    " + URWP);
        // print(DLWP + "    " + DRWP);
        // print(ammoPos);
        return ammoPos.x >= DLWP.x && ammoPos.y >= DLWP.y && ammoPos.x <= URWP.x && ammoPos.y <= URWP.y;
    }
}
