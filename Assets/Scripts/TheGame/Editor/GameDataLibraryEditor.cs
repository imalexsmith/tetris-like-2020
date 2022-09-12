using UnityEditor;
using UnityEngine;
using TheGame;

// ========================
// Revision 2020.10.16
// ========================

[CustomEditor(typeof(GameDataLibrary))]
public class GameDataLibraryEditor : CustomEditorBase<GameDataLibrary>
{
    // ========================================================================================
    [MenuItem("TheGame/GameDataDB", priority = 11)]
    private static void CreateOrSelectAsset()
    {
        EditorExtend.CreateUniqueScriptableObjectOrSelect<GameDataLibrary>(Exceptions.GameDataLibraryMustBeUnique);
    }

    [InitializeOnLoadMethod]
    private static void LoadAsset()
    {
        EditorExtend.LoadUniqueScriptableObject<GameDataLibrary>();
    }


    // ========================================================================================
    private SerializedProperty _blockPrefabsProperty;
    private SerializedProperty _figureTemplatesProperty;


    // ========================================================================================
    protected override void OnEnable()
    {
        base.OnEnable();

        _blockPrefabsProperty = serializedObject.FindProperty("_blockPrefabs");
        _figureTemplatesProperty = serializedObject.FindProperty("_figureTemplates");

        ShowMessage = false;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.UpdateIfRequiredOrScript();

        this.DrawEnumsBlock<FigureBlockColorKeys>(_blockPrefabsProperty);
        this.DrawEnumsBlock<FigureTypeKeys>(_figureTemplatesProperty);

        this.DrawAllProperties(new[] { "_blockPrefabs", "_figureTemplates" });

        serializedObject.ApplyModifiedProperties();
    }
}
