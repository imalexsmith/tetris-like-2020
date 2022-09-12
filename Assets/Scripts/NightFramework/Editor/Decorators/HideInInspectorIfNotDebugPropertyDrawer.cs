using UnityEngine;
using UnityEditor;
using NightFramework;

// ========================
// Revision 2019.11.05
// ========================

[CustomPropertyDrawer(typeof (HideInInspectorIfNotDebugAttribute))]
public class HideInInspectorIfNotDebugPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0F;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
    }
}