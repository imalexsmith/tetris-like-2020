using UnityEditor;
using UnityEngine;
using NightFramework;

// ========================
// Revision 2020.04.01
// ========================

[CustomPropertyDrawer(typeof(PropertyEffectInt)), CustomPropertyDrawer(typeof(PropertyEffectFloat)), CustomPropertyDrawer(typeof(PropertyEffectBool))]
public class PropertyEffectPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var w1 = EditorGUI.indentLevel * 15f;
        var w2 = position.width / 3f;
        var w3 = w1 / 3f;

        position.x -= 15f;
        position.width += 15f;

        var r1 = new Rect(position.x, position.y, 15f + w1, position.height);
        var r2 = new Rect(position.x + 15f, position.y, w2 + w1 - w3, position.height);
        var r3 = new Rect(position.x + 15f + w2 - w3, position.y, w2 + w1 - w3, position.height);
        var r4 = new Rect(position.x + 15f + (w2 - w3) * 2f, position.y, w2 + w1 - w3, position.height);

        var applicable = property.FindPropertyRelative(nameof(PropertyEffectInt.Applicable));
        EditorGUI.PropertyField(r1, applicable, GUIContent.none);
        EditorGUI.LabelField(r2, label);
        if (applicable.boolValue)
        {
            EditorGUI.PropertyField(r3, property.FindPropertyRelative(nameof(PropertyEffectInt.EffectType)), GUIContent.none);
            EditorGUI.PropertyField(r4, property.FindPropertyRelative(nameof(PropertyEffectInt.Value)), GUIContent.none);
        }

        property.isExpanded = true;
    }
}