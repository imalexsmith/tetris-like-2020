using UnityEngine;
using UnityEditor;
using NightFramework;

// ========================
// Revision 2019.11.05
// ========================

/// <summary>
/// Represent standart property drawer with no option to change value
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new EditorGUI.DisabledScope(true))
            EditorGUI.PropertyField(position, property, label, true);
    }
}