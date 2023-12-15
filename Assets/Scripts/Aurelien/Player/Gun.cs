using UnityEngine;
using UnityEngine.InputSystem;
using Aurinaxtailer;
using System.Collections.Generic;
using TMPro;

public class Gun : MonoBehaviour
{
    [SerializeField] private List<GameObject> ammo;
    [SerializeField] private Transform shootPosition;
    public Transform ShootPosition { get { return shootPosition; } }
    [SerializeField] private float distanceForRecoverAmmo;
    [Tooltip("ball at the start"), SerializeField] private int indexAmmo;
    [SerializeField] private TextMeshProUGUI UIammo;
    [SerializeField] private bool manette;
    private Vector3 manetteDirection;
    private List<Ammo> currentAmmo = new List<Ammo>();

    void Awake()
    {
        for (int i = 0; i < ammo.Count; i++)
            currentAmmo.Add(null);
        if (UIammo == null) return;
        UIammo.text = ammo[indexAmmo].name;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (!context.started || currentAmmo[indexAmmo] != null) return;
        currentAmmo[indexAmmo] = Instantiate(ammo[indexAmmo], shootPosition.position, transform.rotation).GetComponent<Ammo>();
        currentAmmo[indexAmmo]._gun = this;
    }

    public void AmmoRecover(InputAction.CallbackContext context)
    {
        if (!context.started || currentAmmo[indexAmmo] == null || !currentAmmo[indexAmmo].CanRecover) return;
        currentAmmo[indexAmmo].Recover();
    }

    public void ChangeAmmo(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (context.ReadValue<float>() > 0)
            indexAmmo = indexAmmo >= ammo.Count - 1 ? 0 : indexAmmo + 1;
        else
            indexAmmo = indexAmmo <= 0 ? ammo.Count - 1 : indexAmmo - 1;
        if (UIammo == null) return;
        UIammo.text = ammo[indexAmmo].name;
    }

    public void ManetteDirection(InputAction.CallbackContext context)
    {

        manetteDirection = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (manette && manetteDirection != Vector3.zero)
            transform.rotation = Rotation2D.LookToDirection2D(transform.rotation, manetteDirection);
        else if (!manette)
            Rotation2D.LookAtMouse2D(transform);
        if (currentAmmo[indexAmmo] != null && !currentAmmo[indexAmmo].IsRecover && Vector2.Distance(transform.position, currentAmmo[indexAmmo].transform.position) > distanceForRecoverAmmo)
            currentAmmo[indexAmmo].CanRecover = true;
    }
}
