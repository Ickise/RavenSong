using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BallInterfacePosition : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    private Camera cam;
    private Vector3 DLWP, ULWP, DRWP, URWP;
    private List<Vector3> screenAngles = new List<Vector3>();
    private Vector2 intersection;

    void Start()
    {
        cam = Camera.main;
        SetViewportToWorldPoints();
    }

    void Update()
    {
        SetViewportToWorldPoints();
        foreach (var ammo in _gun.CurrentAmmos)
        {
            if (ammo == null) return;
            CalculeIntersectionSegments();
            // print(ammo.transform.position + "  ammo pos");
            // if (IsOnScreen(ammo.transform.position))
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

    private List<Vector2> intersections = new List<Vector2>();
    private void CalculeIntersectionSegments()
    {
        intersections = new List<Vector2>();
        Vector2 C = _gun.transform.position;
        Vector2 D = _gun.CurrentAmmos[0].transform.position;

        for (int i = 0; i < screenAngles.Count - 1; i++)
        {
            Vector2 A = screenAngles[i];
            Vector2 B = screenAngles[i + 1];

            var a = (B.y - A.y) / (B.x - A.x);
            var b = A.y - (a * A.x);

            var c = (D.y - C.y) / (D.x - C.x);
            var d = C.y - (c * C.x);

            intersection.x = (b - d) * (1f / (c - a));
            intersection.y = a * intersection.x + b;
            print(a + "    " + b + "    " + c + "    " + d);
            // print(intersection);
            intersections.Add(intersection);
            // }
            // foreach (var inter in intersections)
            // {

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(DLWP, 1F);
        Gizmos.DrawWireSphere(ULWP, 1F);
        Gizmos.DrawWireSphere(URWP, 1F);
        Gizmos.DrawWireSphere(DRWP, 1F);
        Gizmos.color = Color.green;
        foreach (var item in intersections)
            Gizmos.DrawWireSphere(item, 3F);
    }

    private bool IsOnScreen(Vector3 ammoPos)
    {
        return ammoPos.x > DLWP.x && ammoPos.y > DLWP.y && ammoPos.x < URWP.x && ammoPos.y < URWP.y;
    }
}
