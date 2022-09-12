using UnityEngine;

// ========================
// Revision 2020.10.29
// ========================

namespace TheGame
{
    [CreateAssetMenu(menuName = "TheGame/FigureTemplate")]
    public class FigureTemplate : ScriptableObject
    {
        // ========================================================================================
        public FigureTypeKeys FigureType;
        public FigureBlockColorKeys BlocksColor;
        public Vector2 RotationPivot = new Vector2(2f, 2f);
        public Vector2 VisualSize = new Vector2(4f, 4f);
        public bool[] DefaultBlocks = new bool[Figure.BLOCKS_COUNT];


        // ========================================================================================
    }
}