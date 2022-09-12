using UnityEditor;
using NightFramework.Inputs;

// ========================
// Revision 2019.11.05
// ========================

[CustomEditor(typeof(InputButtonHandler), true)]
public class InputButtonHandlerEditor : CustomEditorBase<InputButtonHandler>
{
    // ========================================================================================
    private SerializedProperty _handlersGroupProperty;
    private SerializedProperty _searchGroupInParentOnStartProperty;
    private SerializedProperty _inputDeviceProperty;
    private SerializedProperty _buttonNameProperty;
    private EditorGUIPainter _buttonNamePainter;

    private string[] _axesNames;


    // ========================================================================================
    protected override void OnEnable()
    {
        base.OnEnable();

        _handlersGroupProperty = serializedObject.FindProperty("HandlersGroup");
        _searchGroupInParentOnStartProperty = serializedObject.FindProperty("SearchGroupInParentOnStart");
        _inputDeviceProperty = serializedObject.FindProperty("InputDevice");
        _buttonNameProperty = serializedObject.FindProperty("ButtonName");
        _buttonNamePainter = new EditorGUIPainter();

        _axesNames = EditorExtend.ReadInputAxes();

        ShowMessage = false;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.UpdateIfRequiredOrScript();


        EditorGUILayout.PropertyField(_handlersGroupProperty);
        EditorGUILayout.PropertyField(_searchGroupInParentOnStartProperty);
        EditorGUILayout.PropertyField(_inputDeviceProperty);

        _buttonNamePainter.Begin(this);
        EditorGUILayout.PropertyField(_buttonNameProperty);
        _buttonNamePainter.End();

        var oldVal = -1;
        _buttonNamePainter.HasWarning = true;

        for (int i = 0; i < _axesNames.Length; i++)
        {
            if (Target.ButtonName == _axesNames[i])
            {
                oldVal = i;
                _buttonNamePainter.HasWarning = false;
                break;
            }
        }

        var newVal = EditorGUILayout.Popup(" ", oldVal, _axesNames);
        if (newVal != oldVal)
            _buttonNameProperty.stringValue = _axesNames[newVal];


        this.DrawAllProperties(new[] { "HandlersGroup", "SearchGroupInParentOnStart", "InputDevice", "ButtonName" });

        serializedObject.ApplyModifiedProperties();
    }
}

