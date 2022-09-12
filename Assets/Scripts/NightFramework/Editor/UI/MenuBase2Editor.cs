using UnityEditor;
using NightFramework.UI;
using UnityEngine;
using CES = CustomEditorStyles;

// ========================
// Revision 2020.10.17
// ========================

[CustomEditor(typeof(MenuBase2), true)]
public class MenuBase2Editor : CustomEditorBase<MenuBase2>
{
    // ========================================================================================
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.UpdateIfRequiredOrScript();

        if (EditorApplication.isPlaying)
        {
            var count = MenuBase2.OpenedMenus.Count;

            InformationMessageHeader = $"Opened menus ({count}):";
            if (count > 0)
            {
                foreach (var menu in MenuBase2.OpenedMenus)
                    InformationMessage += $"  • {menu.gameObject.name} ({menu.GetType().Name});\n";
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open", CES.MiniButtonLeftWithNoPadding))
                Target.Open();
            if (GUILayout.Button("Close", CES.MiniButtonMidWithNoPadding))
                Target.Close();
            if (GUILayout.Button("Open (no anim)", CES.MiniButtonMidWithNoPadding))
                Target.Open(false);
            if (GUILayout.Button("Close (no anim)", CES.MiniButtonRightWithNoPadding))
                Target.Close(false);
            EditorGUILayout.EndHorizontal();
        }
        else
            InformationMessageHeader = $"Opened menus (...):";

        this.DrawAllProperties();

        serializedObject.ApplyModifiedProperties();
    }
}
