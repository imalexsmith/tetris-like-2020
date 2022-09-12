using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NightFramework;
using UnityEditor;
using UnityEngine;
using CES = CustomEditorStyles;

// ========================
// Revision 2020.09.27
// ========================

public static class EditorExtensionMethods
{
    // ========================================================================================
    public static void DrawAllProperties(this Editor editor, string[] except = null, bool withInternal = false)
    {
        var checkArray = !(except == null || except.Length == 0);

        var sp = editor.serializedObject.GetIterator();
        sp.Next(true);
        while (true)
        {
            var needDraw = false;

            if (sp.name.StartsWith("m_"))
                needDraw = withInternal;
            else
                needDraw = !(checkArray && except.Contains(sp.name));

            if (needDraw)
                EditorGUILayout.PropertyField(sp, sp.hasVisibleChildren);

            if (!sp.Next(false))
                break;
        }
    }

    public static void DrawSaveLoadFileBlock(this Editor editor)
    {
        var targets = new List<ISaveLoadFile>();
        foreach (var item in editor.targets)
        {
            if (item is ISaveLoadFile target)
                targets.Add(target);
            else
                return;
        }

        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Save to file"))
                {
                    foreach (var item in targets)
                        item.SaveToFile(false);

                    EditorWindow.focusedWindow.ShowNotification(new GUIContent("Saved"));
                }
                if (GUILayout.Button("Load from file"))
                {
                    foreach (var item in targets)
                        item.LoadFromFile();

                    EditorWindow.focusedWindow.ShowNotification(new GUIContent("Loaded"));
                }
            }
            EditorGUILayout.EndHorizontal();

            if (targets.Count == 1)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("File path:", GUILayout.Width(54f));
                    EditorGUILayout.SelectableLabel(targets[0].FileFullPath, GUI.skin.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    if (GUILayout.Button("Open", EditorStyles.miniButtonLeft, GUILayout.Width(48f)))
                    {
                        System.Diagnostics.Process.Start(targets[0].FileFullPath);
                    }
                    if (GUILayout.Button("Copy", EditorStyles.miniButtonRight, GUILayout.Width(48f)))
                    {
                        EditorGUIUtility.systemCopyBuffer = targets[0].FileFullPath;
                        EditorWindow.focusedWindow.ShowNotification(new GUIContent("Copied to clipboard"));
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }

    public static void DrawEnumsBlock<T>(this Editor editor, SerializedProperty property) where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        var maxIndex = (int)values.GetValue(values.Length - 1);
        var numbColor = maxIndex == values.Length - 1 ? Color.green : new Color(1f, 0.66f, 0.33f);
        var tmpColor = GUI.color; 

        EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"{typeof(T).Name}", CES.CenteredBoldLabelStyle);

            EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("total IDs:", GUILayout.Width(51f));
                GUI.color = numbColor;
                EditorGUILayout.LabelField($"{values.Length}", GUILayout.MinWidth(10f), GUILayout.MaxWidth(30f));
                GUI.color = tmpColor;

                GUILayout.Space(4f);

                EditorGUILayout.LabelField("max ID:", GUILayout.Width(42f));
                GUI.color = numbColor;
                EditorGUILayout.LabelField($"{maxIndex}", GUILayout.MinWidth(10f), GUILayout.MaxWidth(30f));
                GUI.color = tmpColor;
                GUILayout.FlexibleSpace();

                var collapse = GUILayout.Button(new GUIContent("", CES.MinIcon, "Collapse all"), CES.MiniButtonLeftWithNoPadding, GUILayout.Width(19f), GUILayout.Height(16f));
                var expand = GUILayout.Button(new GUIContent("", CES.MaxIcon, "Expand all"), CES.MiniButtonMidWithNoPadding, GUILayout.Width(19f), GUILayout.Height(16f));
                var showUndef = property.isExpanded;
                if (showUndef)
                {
                    if (GUILayout.Button(new GUIContent("", CES.HideIcon, "Hide undefined IDs"), CES.MiniButtonRightWithNoPadding, GUILayout.Width(19f), GUILayout.Height(16f)))
                        showUndef = false;
                }
                else
                {
                    if (GUILayout.Button(new GUIContent("", CES.ShowIcon, "Show undefined IDs"), CES.MiniButtonRightWithNoPadding, GUILayout.Width(19f), GUILayout.Height(16f)))
                        showUndef = true;
                }
                property.isExpanded = showUndef;
            EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUI.indentLevel++;
        var count = Mathf.Max(property.arraySize, maxIndex + 1);
        for (int i = 0; i < count; i++)
        {
            var name = Enum.GetName(typeof(T), i);
            var undef = string.IsNullOrEmpty(name);
            var label = new GUIContent($"{i} {(undef ? "<undefined ID>" : name)}");

            if (i >= property.arraySize)
                property.InsertArrayElementAtIndex(i);

            var arrayElement = property.GetArrayElementAtIndex(i);

            if (undef && !showUndef)
                continue;

            if (undef)
                GUI.color = new Color(1f, 0.66f, 0.33f);

            var expanded = EditorGUILayout.Foldout(arrayElement.isExpanded, label, true);

            GUI.color = tmpColor;

            if (collapse)
                expanded = false;
            if (expand)
                expanded = true;

            arrayElement.isExpanded = expanded;
            
            if (expanded)
            {
                EditorGUI.indentLevel++;
                if (!arrayElement.hasVisibleChildren)
                {
                    EditorGUILayout.PropertyField(arrayElement, GUIContent.none);
                }
                else
                {
                    foreach (var item in arrayElement)
                    {
                        var p = (SerializedProperty)item;
                        if (p.hasVisibleChildren)
                            EditorGUILayout.LabelField(p.name);
                        else
                            EditorGUILayout.PropertyField(p);
                    }
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
        }
        EditorGUI.indentLevel--;
    }

    public static void OpenInNewWindow(this Editor editor)
    {
        var target = editor.target;

        // Get a reference to the `InspectorWindow` type object
        var inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
        // Create an InspectorWindow instance
        var inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as EditorWindow;
        // We display it - currently, it will inspect whatever gameObject is currently selected
        // So we need to find a way to let it inspect/aim at our target GO that we passed
        // For that we do a simple trick:
        // 1- Cache the current selected gameObject
        // 2- Set the current selection to our target GO (so now all inspectors are targeting it)
        // 3- Lock our created inspector to that target
        // 4- Fallback to our previous selection
        inspectorInstance.Show();
        // Cache previous selected gameObject
        var prevSelection = Selection.activeGameObject;
        // Set the selection to GO we want to inspect
        Selection.activeObject = target;
        // Get a ref to the "locked" property, which will lock the state of the inspector to the current inspected target
        var isLocked = inspectorType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.Public);
        // Invoke `isLocked` setter method passing 'true' to lock the inspector
        isLocked.GetSetMethod().Invoke(inspectorInstance, new object[] { true });
        // Finally revert back to the previous selection so that other inspectors continue to inspect whatever they were inspecting...
        Selection.activeGameObject = prevSelection;
    }
}
