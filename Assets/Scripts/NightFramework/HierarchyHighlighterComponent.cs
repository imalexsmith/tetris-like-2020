//HierarchyHighlighterComponent.cs
using UnityEngine;

[DisallowMultipleComponent]
public class HierarchyHighlighterComponent : MonoBehaviour
{
    //attach this to the gameobject you want to highlight in the hierarchy

    public bool highlight = true;
    public Color colorFrom = new Color(0.125f, 0.25f, 1f, 0f);
    public Color colorTo = new Color(0.125f, 0.25f, 1f, 1f);
    [Range(0f, 0.95f)]
    public float gradientStart = 0.5f;
}