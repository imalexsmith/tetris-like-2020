using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using NightFramework;

// ========================
// Revision 2020.11.11
// ========================

namespace TheGame
{
    [Serializable]
    public struct BestScoresEntry : IComparable<BestScoresEntry>
    {
        public int Score;

        public int CompareTo(BestScoresEntry other)
        {
            return -Score.CompareTo(other.Score);
        }
    }


    public class PersistentDataStorage : ScriptableObject, ISaveLoadFile
    {
        public const string FILE_NAME = "perstData.txt";
        public const int MAX_BEST_SCORES = 10;


        // ========================================================================================
        private static PersistentDataStorage _instance;
        public static PersistentDataStorage Instance
        {
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
            get
            {
                if (_instance == null)
                {
                    var assets = Resources.FindObjectsOfTypeAll<PersistentDataStorage>();
                    if (assets.Length == 1)
                        _instance = assets[0];
                    else
                        throw new UnityException(Exceptions.PersistentDataStorageMustBeUnique);
                }

                return _instance;
            }
        }


        // ========================================================================================
        public string FileFullPath => Path.Combine(Application.persistentDataPath, FILE_NAME);

        public List<BestScoresEntry> BestScores = new List<BestScoresEntry>(MAX_BEST_SCORES);


        // ========================================================================================
        public void SaveToFile(bool compressed = true)
        {
            var dat = JsonUtility.ToJson(this, !compressed);
            File.WriteAllText(FileFullPath, dat, Encoding.UTF8);
        }

        public bool LoadFromFile()
        {
            if (File.Exists(FileFullPath))
            {
                var text = File.ReadAllText(FileFullPath, Encoding.UTF8);
                JsonUtility.FromJsonOverwrite(text, this);
                return true;
            }

            return false;
        }

        protected void Reset()
        {
            BestScores = new List<BestScoresEntry>(MAX_BEST_SCORES);
            for (int i = 0; i < MAX_BEST_SCORES; i++)
                BestScores.Add(new BestScoresEntry() { Score = i * 10000 });
            BestScores.Sort();
        }
    }
}