using UnityEditor;
using NightFramework.Inputs;

// ========================
// Revision 2019.11.05
// ========================

[CustomEditor(typeof(InputHandlersGroup), true), CanEditMultipleObjects]
public class InputHandlersGroupEditor : CustomEditorBase<InputHandlersGroup>
{
    // ========================================================================================
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.UpdateIfRequiredOrScript();

        var count = InputHandlersGroup.AvailableHandlersGroups.Count;

        InformationMessageHeader = string.Format("Available handlers groups ({0})", count);
        if (count > 0)
        {
            foreach (var group in InputHandlersGroup.AvailableHandlersGroups)
            {
                InformationMessage += string.Format("  {0} {1} ({2});\n",
                                                 group == InputHandlersGroup.ActiveGroup ? "+" : "-",
                                                 group.gameObject.name,
                                                 group.GetType().Name);
            }
        }

        this.DrawAllProperties();

        serializedObject.ApplyModifiedProperties();
    }
}
