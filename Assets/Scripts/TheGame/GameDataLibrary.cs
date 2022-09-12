using System.Collections.Generic;
using UnityEngine;

// ========================
// Revision 2020.10.16
// ========================

namespace TheGame
{
    public class GameDataLibrary : ScriptableObject
    {
        // ========================================================================================
        private static GameDataLibrary _instance;
        public static GameDataLibrary Instance
        {
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
            get
            {
                if (_instance == null)
                {
                    var assets = Resources.FindObjectsOfTypeAll<GameDataLibrary>();
                    if (assets.Length == 1)
                        _instance = assets[0];
                    else
                        throw new UnityException(Exceptions.GameDataLibraryMustBeUnique);
                }

                return _instance;
            }
        }


        // ========================================================================================
        [SerializeField]
        private List<FigureBlock> _blockPrefabs;
        public FigureBlock this[FigureBlockColorKeys key] => _blockPrefabs[(int)key];

        [SerializeField]
        private List<FigureTemplate> _figureTemplates;
        public FigureTemplate this[FigureTypeKeys key] => _figureTemplates[(int)key];


        // ========================================================================================
        public GameDataLibrary()
        {
            Enums.RecreateList(ref _blockPrefabs, typeof(FigureBlockColorKeys));
            Enums.RecreateList(ref _figureTemplates, typeof(FigureTypeKeys));
        }
    }
}