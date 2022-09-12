using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using NightFramework;

// ========================
// Revision 2020.10.15
// ========================

[CustomPropertyDrawer(typeof(RandomisedSet<>))]
public class RandomisedSetPropertyDrawer : PropertyDrawer
{
    private const float VListPadding = 2f;
    private const float ToggleWidth = 13f;
    

    // ========================================================================================
    private Dictionary<string, ReorderableList> _lists = new Dictionary<string, ReorderableList>();


    // ========================================================================================
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var listProperty = property.FindPropertyRelative(nameof(RandomisedSet<object>.Values));
        return GetList(property, listProperty).GetHeight();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = new Rect(position.x + EditorGUI.indentLevel * 15f, position.y, position.width - EditorGUI.indentLevel * 15f, position.height);
        
        var listProperty = property.FindPropertyRelative(nameof(RandomisedSet<object>.Values));
        GetList(property, listProperty).DoList(position);
    }

    private ReorderableList GetList(SerializedProperty baseProperty, SerializedProperty listProperty)
    {
        if (!_lists.TryGetValue(baseProperty.propertyPath, out var result))
        {
            result = new ReorderableList(baseProperty.serializedObject, listProperty, true, true, true, true);
            _lists.Add(baseProperty.propertyPath, result);

            result.drawElementCallback += (rect, index, active, focused) =>
            {
                if (baseProperty.isExpanded)
                {
                    var ind = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;

                    var item = listProperty.GetArrayElementAtIndex(index);

                    var p1 = item.FindPropertyRelative(nameof(RandomisedSetEntry<object>.Value));
                    var p2 = item.FindPropertyRelative(nameof(RandomisedSetEntry<object>.Always));

                    var r1 = new Rect(rect.x, rect.y + VListPadding, rect.width, EditorGUIUtility.singleLineHeight);
                    var r2 = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + VListPadding * 2, rect.width, EditorGUIUtility.singleLineHeight);

                    EditorGUI.PropertyField(r1, p1, new GUIContent(p1.displayName));
                    EditorGUI.PropertyField(r2, p2, new GUIContent(p2.displayName));

                    if (!p2.boolValue)
                    {
                        var p3 = item.FindPropertyRelative(nameof(RandomisedSetEntry<object>.Weight));
                        p3.floatValue = Mathf.Max(p3.floatValue, 0f);

                        var r3 = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2 + VListPadding * 3, rect.width, EditorGUIUtility.singleLineHeight);

                        EditorGUI.PropertyField(r3, p3, new GUIContent(p3.displayName));
                    }

                    EditorGUI.indentLevel = ind;
                }
            };
            result.elementHeightCallback += (index) =>
            {
                if (baseProperty.isExpanded)
                {
                    var item = listProperty.GetArrayElementAtIndex(index);

                    var p = item.FindPropertyRelative(nameof(RandomisedSetEntry<object>.Always));

                    if (!p.boolValue)
                        return (EditorGUIUtility.singleLineHeight + VListPadding) * 3f + VListPadding;
                    else
                        return (EditorGUIUtility.singleLineHeight + VListPadding) * 2f + VListPadding;
                }

                return 0f;
            };
            result.drawHeaderCallback += (rect) =>
            {
                var expanded = baseProperty.isExpanded;

                var ind = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                var p2 = baseProperty.FindPropertyRelative(nameof(RandomisedSet<object>.SelectionRounds));
                p2.intValue = Mathf.Max(p2.intValue, 1);
                var p3 = baseProperty.FindPropertyRelative(nameof(RandomisedSet<object>.SelectionMode));

                var drawRounds = false;
                var mode = (RandomValueSelectionMode)p3.enumValueIndex;
                if (mode == RandomValueSelectionMode.SeveralWithRepeats || mode == RandomValueSelectionMode.SeveralWithNoRepeats)
                    drawRounds = true;

                var rToggle = new Rect(rect.x, rect.y, rect.width - 140f, rect.height);
                var r1 = new Rect(rect.x + ToggleWidth, rect.y, rect.width - ToggleWidth - 140f, rect.height);
                var r2 = new Rect(r1.x + r1.width, rect.y, 27f, rect.height);
                var r3 = new Rect(r2.x + r2.width, rect.y, 113f, rect.height);

                EditorExtend.MakeReorderableListHeaderFoldout(result, rToggle, ref expanded);
                EditorGUI.LabelField(r1, new GUIContent($"[{result.count}] {baseProperty.displayName}", ""));
                if (drawRounds)
                {
                    EditorGUI.PropertyField(r2, p2, GUIContent.none);
                    EditorGUI.LabelField(r2, new GUIContent("", p2.displayName));
                }
                EditorGUI.PropertyField(r3, p3, new GUIContent("", p3.displayName));
                EditorGUI.LabelField(r3, new GUIContent("", p3.displayName));

                EditorGUI.indentLevel = ind;

                baseProperty.isExpanded = expanded;
            };
            
            EditorExtend.SwitchReorderableListFoldout(result, baseProperty.isExpanded);
        }

        return result;
    }
}