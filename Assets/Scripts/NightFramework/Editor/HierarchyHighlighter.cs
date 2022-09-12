//HierarchyHighlighter.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using CES = CustomEditorStyles;

[InitializeOnLoad]
public class HierarchyHighlighter
{
    static HierarchyHighlighter()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItem_CB;
    }

    private static void HierarchyWindowItem_CB(int selectionID, Rect selectionRect)
    {
        var o = EditorUtility.InstanceIDToObject(selectionID);
        var go = o as GameObject;
        if (go != null)
        {
            var hhc = go.GetComponent<HierarchyHighlighterComponent>();
            if (hhc != null)
            {
                if (hhc.highlight)
                {
                    var r = new Rect(selectionRect.x + selectionRect.width * hhc.gradientStart,
                                     selectionRect.y, 
                                     selectionRect.width * (1f - hhc.gradientStart), 
                                     selectionRect.height);

                    GUI.DrawTexture(r, CES.MakeGradientTexture((int)r.width, (int)r.height, hhc.colorFrom, hhc.colorTo));

                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }
    }
}