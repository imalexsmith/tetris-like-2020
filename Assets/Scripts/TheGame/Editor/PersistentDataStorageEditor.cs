using UnityEditor;
using UnityEngine;
using TheGame;

// ========================
// Revision 2020.11.04
// ========================

[CustomEditor(typeof(PersistentDataStorage))]
public class PersistentDataStorageEditor : CustomEditorBase<PersistentDataStorage>
{
    // ========================================================================================
    [MenuItem("TheGame/PersistentDataStorage", priority = 12)]
    private static void CreateOrSelectAsset()
    {
        EditorExtend.CreateUniqueScriptableObjectOrSelect<PersistentDataStorage>(Exceptions.PersistentDataStorageMustBeUnique);
    }

    [InitializeOnLoadMethod]
    private static void LoadAsset()
    {
        EditorExtend.LoadUniqueScriptableObject<PersistentDataStorage>();
    }


    // ========================================================================================


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
