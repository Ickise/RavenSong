#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
[InitializeOnLoad]
public class HierarchyGroupHeader : Editor
{
    static HierarchyGroupHeader()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyGroupOnGUI;
    }
    static void HierarchyGroupOnGUI(int instanceID, Rect selectionRect)
    {
        var gameobject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        
        if(gameobject !=null)
        {
            if(gameobject.name.StartsWith("---", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.green);
                EditorGUI.DropShadowLabel(selectionRect, gameobject.name.Replace("-", "").ToUpperInvariant());
            }
            if(gameobject.name.StartsWith("----", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.cyan);
                EditorGUI.DropShadowLabel(selectionRect, gameobject.name.Replace("-", "").ToUpperInvariant());
            }
            if(gameobject.name.StartsWith("-----", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.yellow);
                EditorGUI.DropShadowLabel(selectionRect, gameobject.name.Replace("-", "").ToUpperInvariant());
            }
            if(gameobject.name.StartsWith("------", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.red);
                EditorGUI.DropShadowLabel(selectionRect, gameobject.name.Replace("-", "").ToUpperInvariant());
            }
            if(gameobject.name.StartsWith("-------", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.white);
                EditorGUI.DropShadowLabel(selectionRect, gameobject.name.Replace("-", "").ToUpperInvariant());
            }
            if(gameobject.name.StartsWith("--------", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.magenta);
                EditorGUI.DropShadowLabel(selectionRect, gameobject.name.Replace("-", "").ToUpperInvariant());
            }
        }
    }
}
#endif
