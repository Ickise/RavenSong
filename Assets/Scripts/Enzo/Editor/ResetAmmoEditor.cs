using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResetAmmo))]
[CanEditMultipleObjects]
public class ResetAmmoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ResetAmmo _resetAmmo = (ResetAmmo)target;


        if (GUILayout.Button("ResetNumberOfAmmo"))
        {
            _resetAmmo.ResetBullet();
        }
    }
}