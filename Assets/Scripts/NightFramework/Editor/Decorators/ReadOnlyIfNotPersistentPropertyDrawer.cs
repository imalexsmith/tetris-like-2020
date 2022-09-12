using UnityEngine;
using UnityEditor;
using NightFramework;

// ========================
// Revision 2019.11.05
// ========================

/// <summary>
/// Represent standart property drawer with no option to change value
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyIfNotPersistentAttribute))]
public class ReadOnlyIfNotPersistentPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (EditorUtility.IsPersistent(property.serializedObject.targetObject))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        else
        {
            using (new EditorGUI.DisabledScope(true))
                EditorGUI.PropertyField(position, property, label, true);
        }
    }
}