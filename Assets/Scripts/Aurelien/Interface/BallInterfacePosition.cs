using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class BallInterfacePosition : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    private Camera cam;
    private CinemachineVirtualCamera virtualCam;
    private Vector3 DLWP, ULWP, DRWP, URWP;

    void Start()
    {
        cam = Camera.main;
        SetViewportToWorldPoints();
    }

    void Update()
    {
        print(DLWP + "  bas gauche");
        print(URWP + "  haut droit");
        SetViewportToWorldPoints();
        foreach (var ammo in _gun.CurrentAmmos)
        {
            if (ammo == null) return;
            print(ammo.transform.position + "  ammo pos");
            if (IsOnScreen(ammo.transform.position))
                Indicator(ammo);
        }
    }

    private void Indicator(Ammo ammo)
    {
        // print(Camera.main.WorldToViewportPoint(ammo.transform.position));
    }

    private void SetViewportToWorldPoints()
    {
        DLWP = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        ULWP = cam.ViewportToWorldPoint(Vector2.up);
        DRWP = cam.ViewportToWorldPoint(Vector2.right);
        URWP = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        // virtualCam.
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(DLWP, 1F);
        Gizmos.DrawWireSphere(URWP, 1F);
    }

    private bool IsOnScreen(Vector3 ammoPos)
    {
        return ammoPos.x > DLWP.x && ammoPos.y > DLWP.y && ammoPos.x < URWP.x && ammoPos.y < URWP.y;
    }
}
