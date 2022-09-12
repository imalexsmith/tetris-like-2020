using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using NightFramework.UI;

// ========================
// Revision 2020.03.02
// ========================

[CustomEditor(typeof(TextLocalizatorTMP))]
public class TextLocalizatorTMPEditor : TextLocalizatorEditor<TMP_Text> { }

[CustomEditor(typeof(TextLocalizatorUGUI))]
public class TextLocalizatorUGUIEditor : TextLocalizatorEditor<Text> { }

public abstract class TextLocalizatorEditor<T> : CustomEditorBase<TextLocalizator<T>> where T : UIBehaviour
{
    // ========================================================================================
    private SerializedProperty _onTranslatedProperty;
    private SerializedProperty _localizationPackProperty;
    private SerializedProperty _keyProperty;
    private EditorGUIPainter _keyPainter;

    private List<string> _keys;
    private string _translationPreview;


    // ========================================================================================
    protected override void OnEnable()
    {
        base.OnEnable();

        _onTranslatedProperty = serializedObject.FindProperty("OnTranslated");
        _localizationPackProperty = serializedObject.FindProperty("LocalizationPack");
        _keyProperty = serializedObject.FindProperty("_key");
        _keyPainter = new EditorGUIPainter();

        _keys = new List<string>();
        OnChangeProperty();

        if (!serializedObject.isEditingMultipleObjects)
            InformationMessageHeader = "Translation preview:";
        else
            ShowMessage = false;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.PropertyField(_onTranslatedProperty);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_localizationPackProperty);
        if (EditorGUI.EndChangeCheck())
            OnChangeProperty();

        this.DrawAllProperties(new[] { "OnTranslated", "LocalizationPack", "_key" });

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.Height(EditorGUIUtility.singleLineHeight * 7f));
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.labelWidth));
            {
                EditorGUILayout.PrefixLabel("Key");
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Random", EditorStyles.miniButtonLeft, GUILayout.Width(52f)))
                    {
                        if (Target.LocalizationPack != null)
                        {
                            var keys = Target.LocalizationPack.GetKeys().ToList();
                            var i = Random.Range(0, keys.Count);

                            _keyProperty.stringValue = keys[i];
                        }
                    }
                    if (GUILayout.Button("Translate", EditorStyles.miniButtonRight, GUILayout.Width(60f)))
                    {
                        if (Target.LocalizationPack == null || string.IsNullOrWhiteSpace(Target.Key))
                            return;

                        Target.Translate();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            _keyPainter.Begin(this);
            _keyProperty.stringValue = EditorExtend.TextFieldAutoComplete(_keyProperty.stringValue, _keys, 6, 0.75f);
            _keyPainter.End();
        }
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
            OnChangeProperty();

        _keyPainter.HasWarning = _keyProperty.stringValue.Length == 0;
        _keyPainter.HasError = _keyProperty.stringValue.Length > 0 && !_keys.Contains(Target.Key);

        InformationMessage = _translationPreview;

        serializedObject.ApplyModifiedProperties();
    }

    private void OnChangeProperty()
    {
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();

        if (!serializedObject.isEditingMultipleObjects && Target.LocalizationPack != null && !string.IsNullOrWhiteSpace(Target.Key))
        {
            _keys = Target.LocalizationPack.GetKeys().ToList();
            _translationPreview = Target.LocalizationPack.GetEntries(Target.Key).Aggregate("", (initial, next) => initial += $"[{next.Language}] {next.Value}\n");
            return;
        }

        _keys.Clear();
        _translationPreview = "";
    }
}

