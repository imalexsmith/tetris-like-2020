using UnityEditor;
using UnityEngine;
using NightFramework;
using TheGame;

// ========================
// Revision 2020.10.22
// ========================

[CustomEditor(typeof(ApplicationSettingsBase), true)]
public class ApplicationSettingsBaseEditor : CustomEditorBase<ApplicationSettingsBase>
{
    // ========================================================================================
    [MenuItem("TheGame/ApplicationSettings", priority = 10)]
    private static void CreateOrSelectAsset()
    {
        EditorExtend.CreateUniqueScriptableObjectOrSelect<ApplicationSettings>(NightFramework.Exceptions.ApplicationSettingsMustBeUnique);
    }

    [InitializeOnLoadMethod]
    private static void LoadAsset()
    {
        EditorExtend.LoadUniqueScriptableObject<ApplicationSettings>();
    }


    // ========================================================================================
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.UpdateIfRequiredOrScript();

        WarningMessage = "<b>Do not forget to press \"Reset\" before publishing!</b>";

        this.DrawAllProperties();
        this.DrawSaveLoadFileBlock();

        serializedObject.ApplyModifiedProperties();
    }
}
