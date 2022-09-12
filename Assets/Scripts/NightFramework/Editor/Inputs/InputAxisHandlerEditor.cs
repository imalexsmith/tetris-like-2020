using UnityEditor;
using NightFramework.Inputs;

// ========================
// Revision 2019.11.05
// ========================

[CustomEditor(typeof(InputAxisHandler), true)]
public class InputAxisHandlerEditor : CustomEditorBase<InputAxisHandler>
{
    // ========================================================================================
    private SerializedProperty _handlersGroupProperty;
    private SerializedProperty _searchGroupInParentOnStartProperty;
    private SerializedProperty _inputDeviceProperty;
    private SerializedProperty _axisNameProperty;
    private EditorGUIPainter _axisNamePainter;
    
    private string[] _axesNames;


    // ========================================================================================
    protected override void OnEnable()
    {
        base.OnEnable();

        _handlersGroupProperty = serializedObject.FindProperty("HandlersGroup");
        _searchGroupInParentOnStartProperty = serializedObject.FindProperty("SearchGroupInParentOnStart");
        _inputDeviceProperty = serializedObject.FindProperty("InputDevice");
        _axisNameProperty = serializedObject.FindProperty("AxisName");
        _axisNamePainter = new EditorGUIPainter();

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

        _axisNamePainter.Begin(this);
        EditorGUILayout.PropertyField(_axisNameProperty);
        _axisNamePainter.End();

        var oldVal = -1;
        _axisNamePainter.HasWarning = true;

        for (int i = 0; i < _axesNames.Length; i++)
        {
            if (Target.AxisName == _axesNames[i])
            {
                oldVal = i;
                _axisNamePainter.HasWarning = false;
                break;
            }
        }

        var newVal = EditorGUILayout.Popup(" ", oldVal, _axesNames);
        if (newVal != oldVal)
            _axisNameProperty.stringValue = _axesNames[newVal];


        this.DrawAllProperties(new[] { "HandlersGroup", "SearchGroupInParentOnStart", "InputDevice", "AxisName" });

        serializedObject.ApplyModifiedProperties();
    }
}
