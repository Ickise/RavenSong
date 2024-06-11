using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class BallInterfacePosition : MonoBehaviour
{
    [SerializeField] private FireOneBullet _gun;
    [SerializeField, Tooltip("A quel point l'indicateur est proche du joueur")]
    private float indicatorScaleOnScreen = 0.97f;
    private Camera cam;
    private Vector3 DLWP, ULWP, DRWP, URWP;
    private List<Vector3> screenAngles = new List<Vector3>();
    [SerializeField] private GameObject indicator;

    void Start()
    {
        cam = Camera.main;
        SetViewportToWorldPoints();
        _gun.transform.parent.GetComponent<RecallBullet>()._ballInterfacePosition = this;
    }

    void FixedUpdate()
    {
        if (_gun == null || _gun.bulletRef == null)
        {
            indicator.SetActive(false);
            return;
        }
        SetViewportToWorldPoints();
        Indicator();
    }

    private void Indicator()
    {
        Transform bullet = _gun.bulletRef.transform;
        if (!IsOnScreen(bullet.position))
        {
            var intersectionAmmo = CalculeIntersectionSegments(bullet.position);
            indicator.transform.gameObject.SetActive(true);
            indicator.transform.position = intersectionAmmo;
            indicator.transform.localScale = 10f / Vector2.Distance(_gun.transform.position, bullet.position) * Vector3.one;
            indicator.transform.localScale = Vector3.one * Mathf.Clamp(indicator.transform.localScale.x, 0.4f, Mathf.Infinity);
        }
        else
            indicator.SetActive(false);
    }

    private void SetViewportToWorldPoints()
    {
        DLWP = cam.ViewportToWorldPoint(new Vector3(0, 0, -cam.transform.position.z * indicatorScaleOnScreen));
        ULWP = cam.ViewportToWorldPoint(new Vector3(0, 1, -cam.transform.position.z * indicatorScaleOnScreen));
        URWP = cam.ViewportToWorldPoint(new Vector3(1, 1, -cam.transform.position.z * indicatorScaleOnScreen));
        DRWP = cam.ViewportToWorldPoint(new Vector3(1, 0, -cam.transform.position.z * indicatorScaleOnScreen));
        screenAngles = new List<Vector3>() { DLWP, ULWP, URWP, DRWP, DLWP };
    }

    private Vector2 CalculeIntersectionSegments(Vector2 currentAmmoPos)
    {
        List<Vector2> intersections = new List<Vector2>();
        Vector2 gunPos = _gun.transform.position;
        for (int i = 0; i < screenAngles.Count - 1; i++)
            intersections.Add(Aurinaxtailer.Intersection2D.GetIntersectionBetweenABandCD(screenAngles[i], screenAngles[i + 1], gunPos, currentAmmoPos));

        List<Vector2> interOnScreen = new List<Vector2>();
        foreach (var intersection in intersections)
            if (IsOnScreen(intersection))
                interOnScreen.Add(intersection);

        return Vector2.Distance(interOnScreen[0], currentAmmoPos) < Vector2.Distance(interOnScreen[1], currentAmmoPos) ? interOnScreen[0] : interOnScreen[1];
    }

    private bool IsOnScreen(Vector3 ammoPos)
    {
        return ammoPos.x >= DLWP.x - 0.02f && ammoPos.y >= DLWP.y && ammoPos.x <= URWP.x + 0.02f && ammoPos.y <= URWP.y;
    }

    public void ShakeBallUI()
    {
        indicator.transform.DOShakePosition(0.3f, Vector2.one * 0.2f, 100);
    }
}
