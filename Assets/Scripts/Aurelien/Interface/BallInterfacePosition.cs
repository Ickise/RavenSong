using UnityEngine;
using System.Collections.Generic;

public class BallInterfacePosition : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    private Camera cam;
    private Vector3 DLWP, ULWP, DRWP, URWP;
    private List<Vector3> screenAngles = new List<Vector3>();
    private List<Vector2> intersectionAmmosGizmos = new List<Vector2>();
    [SerializeField] private GameObject ammoIndicatorPrefab;
    private List<GameObject> ammoIndicators = new List<GameObject>();
    private Dictionary<Ammo, GameObject> ammoSpritesDico = new Dictionary<Ammo, GameObject>();

    // [System.Serializable]
    // public struct BallIndicatorSprite
    // {
    //     public Ammo ammo;
    //     public SpriteRenderer ammoSprite;
    // }
    // public BallIndicatorSprite[] ballIndicatorSprites;

    void Start()
    {
        for (int i = 0; i < _gun.CurrentAmmos.Count; i++)
        {
            GameObject AIP = Instantiate(ammoIndicatorPrefab, Vector2.zero, Quaternion.identity);
            if (_gun.CurrentAmmos[i] != null && _gun.CurrentAmmos[i].AmmoSprite)
                AIP.GetComponent<SpriteRenderer>().sprite = _gun.CurrentAmmos[i].AmmoSprite;
            ammoSpritesDico.Add(_gun.CurrentAmmos[i], AIP);
        }
        cam = Camera.main;
        SetViewportToWorldPoints();
    }

    void Update()
    {
        SetViewportToWorldPoints();
        Indicator();
    }

    private void Indicator()
    {
        intersectionAmmosGizmos = new List<Vector2>();
        foreach (var ammo in _gun.CurrentAmmos)
        {
            if (ammo == null || IsOnScreen(ammo.transform.position))
            {
                GameObject ammoSprite;
                ammoSpritesDico.TryGetValue(ammo, out ammoSprite);
                ammoSprite.gameObject.SetActive(false);
            }
            else
            {
                var intersectionAmmo = CalculeIntersectionSegments(ammo.transform.position);
                GameObject ammoSprite;
                ammoSpritesDico.TryGetValue(ammo, out ammoSprite);
                if (ammoSprite.activeInHierarchy)
                    ammoSprite.gameObject.SetActive(true);
                ammoSprite.transform.position = intersectionAmmo;
            }
        }
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
            intersections.Add(Aurinaxtailer.Intersection2D.GetIntersectionBetweenABandCD(screenAngles[i], screenAngles[i + 1], gunPos, currentAmmoPos));

        List<Vector2> interOnScreen = new List<Vector2>();
        foreach (var intersection in intersections)
            if (IsOnScreen(intersection))
                interOnScreen.Add(intersection);

        return Vector2.Distance(interOnScreen[0], currentAmmoPos) < Vector2.Distance(interOnScreen[1], currentAmmoPos) ? interOnScreen[0] : interOnScreen[1];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(DLWP, 1F);
        Gizmos.DrawWireSphere(ULWP, 1F);
        Gizmos.DrawWireSphere(URWP, 1F);
        Gizmos.DrawWireSphere(DRWP, 1F);
        // Gizmos.color = Color.green;
        // foreach (var intersectionAmmo in intersectionAmmosGizmos)
        //     Gizmos.DrawWireSphere(intersectionAmmo, 3F);
    }

    private bool IsOnScreen(Vector3 ammoPos)
    {
        // print(ULWP + "    " + URWP);
        // print(DLWP + "    " + DRWP);
        // print(ammoPos);
        return ammoPos.x >= DLWP.x - 0.02f && ammoPos.y >= DLWP.y && ammoPos.x <= URWP.x + 0.02f && ammoPos.y <= URWP.y;
    }
}
