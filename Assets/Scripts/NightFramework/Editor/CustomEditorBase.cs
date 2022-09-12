using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CES = CustomEditorStyles;

// ========================
// Revision 2019.11.05
// ========================

public abstract class CustomEditorBase<T> : Editor where T : Object
{
    protected const float InfoboxScrollHeight = 90f;
    protected const float MessageIconWidth = 36f;

    // ========================================================================================
    public bool ShowMessage = true;
    public string ErrorMessage;
    public string WarningMessage;
    public string InformationMessageHeader = "Information:";
    public string InformationMessage;

    protected List<T> Targets
    {
        get;
        private set;
    } = new List<T>();

    protected T Target => Targets[0];

    private SerializedProperty _scriptProperty;
    private Vector2 _infoboxScroll;
    private float _infoboxMessageHeight;
    private float _infoboxViewportWidth;
    private Texture2D _errorIcon;
    private Texture2D _warningIcon;


    // ========================================================================================
    protected virtual void OnEnable()
    {
        Targets = serializedObject.targetObjects.Cast<T>().ToList();
        _scriptProperty = serializedObject.FindProperty("m_Script");
        _errorIcon = (Texture2D)EditorGUIUtility.Load("icons/d_console.erroricon.png");
        _warningIcon = (Texture2D)EditorGUIUtility.Load("icons/d_console.warnicon.png");
    }

    public override void OnInspectorGUI()
    {
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.PropertyField(_scriptProperty);

        if (!ShowMessage) 
            return;

        _infoboxScroll = EditorGUILayout.BeginScrollView(_infoboxScroll, GUI.skin.box, GUILayout.Height(InfoboxScrollHeight));

        if (!string.IsNullOrWhiteSpace(ErrorMessage))
        {
            _infoboxMessageHeight = EditorStyles.wordWrappedLabel.CalcHeight(new GUIContent(ErrorMessage), _infoboxViewportWidth);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(_errorIcon, GUILayout.Width(MessageIconWidth));
            EditorGUILayout.SelectableLabel(ErrorMessage, CES.RichWordWrappedLabelStyle, GUILayout.MinHeight(_infoboxMessageHeight));
            if (Event.current.type == EventType.Repaint)
                _infoboxViewportWidth = GUILayoutUtility.GetLastRect().width;
            EditorGUILayout.EndHorizontal();
        }
        else if (!string.IsNullOrWhiteSpace(WarningMessage))
        {
            _infoboxMessageHeight = EditorStyles.wordWrappedLabel.CalcHeight(new GUIContent(WarningMessage), _infoboxViewportWidth);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(_warningIcon, GUILayout.Width(MessageIconWidth));
            EditorGUILayout.SelectableLabel(WarningMessage, CES.RichWordWrappedLabelStyle, GUILayout.MinHeight(_infoboxMessageHeight));
            if (Event.current.type == EventType.Repaint)
                _infoboxViewportWidth = GUILayoutUtility.GetLastRect().width;
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            _infoboxMessageHeight = EditorStyles.wordWrappedLabel.CalcHeight(new GUIContent(InformationMessage), _infoboxViewportWidth);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(InformationMessageHeader, EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(InformationMessage, CES.RichWordWrappedLabelStyle, GUILayout.MinHeight(_infoboxMessageHeight));
            if (Event.current.type == EventType.Repaint)
                _infoboxViewportWidth = GUILayoutUtility.GetLastRect().width;
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        ErrorMessage = "";
        WarningMessage = "";
        InformationMessage = "";
    }
}
