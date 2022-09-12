using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ========================
// Revision 2019.12.18
// ========================

namespace NightFramework
{
    [CreateAssetMenu(menuName = "NightFramework/Localization pack", order = 0)]
    public class Localization : ScriptableObject
    {
        public enum Languages
        {
            EN = 0,
            ZH = 1,
            RU = 2,
            ES = 3,
            DE = 4,
            PT = 5,
            FR = 6
        }

        [Serializable]
        public struct LocalizationEntry
        {
            public string Key;
            public Languages Language;
            public string Value;
        }


        // ===========================================================================================
        [SerializeField]
        private List<LocalizationEntry> _entries = new List<LocalizationEntry>();


        // ===========================================================================================
        public void Clear()
        {
            _entries.Clear();
        }

        public void RestoreMissingEntries()
        {
            var languages = Enum.GetValues(typeof(Languages));

            for (int i = 0; i < languages.Length; i++)
            {
                var lang = (Languages) i;
                RestoreMissingEntries(lang);
            }
        }

        public void RestoreMissingEntries(Languages lang)
        {
            var yes = _entries.Where(x => x.Language == lang).Select(x => x.Key).Distinct();
            var no = _entries.Where(x => x.Language != lang).Select(x => x.Key).Distinct();

            var result = no.Except(yes).ToList();

            foreach (var newKey in result)
            {
                _entries.Add(new LocalizationEntry {Key = newKey, Language = lang, Value = ""});
            }
        }

        public bool AddEntry(LocalizationEntry entry, bool rewrite = false)
        {
            if (string.IsNullOrEmpty(entry.Key)) return false;
            entry.Key = entry.Key.ToLower();

            int i;
            for (i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].Key.Equals(entry.Key) && _entries[i].Language == entry.Language)
                {
                    if (!rewrite)
                        return false;

                    _entries.RemoveAt(i);
                    break;
                }
            }

            _entries.Insert(i, entry);
            return true;
        }

        public bool RemoveEntry(string key, Languages lang)
        {
            if (string.IsNullOrEmpty(key)) return false;
            key = key.ToLower();

            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].Key.Equals(key) && _entries[i].Language == lang)
                {
                    _entries.Remove(_entries[i]);
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<LocalizationEntry> GetEntries(string key = null, Languages? lang = null)
        {
            IEnumerable<LocalizationEntry> result = _entries;

            if (!string.IsNullOrEmpty(key))
            {
                key = key.ToLower();
                result = _entries.Where(x => x.Key == key);
            }

            if (lang != null)
                result = _entries.Where(x => x.Language == lang);

            return result;
        }

        public void AddKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            key = key.ToLower();

            var temp = new List<LocalizationEntry>();
            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].Key.Equals(key))
                    temp.Add(_entries[i]);
            }

            foreach (Languages lang in Enum.GetValues(typeof(Languages)))
            {
                if (temp.Any(x => x.Language == lang))
                    continue;

                _entries.Add(new LocalizationEntry {Key = key, Language = lang});
            }
        }

        public void RemoveKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            key = key.ToLower();

            _entries.RemoveAll(x => x.Key == key);
        }

        public IEnumerable<string> GetKeys(Languages? lang = null)
        {
            IEnumerable<LocalizationEntry> result = _entries;

            result = result.GroupBy(x => x.Key).Select(y => y.First());

            return lang == null
                ? result.Select(x => x.Key)
                : result.Where(x => x.Language == lang).Select(x => x.Key);
        }

        public string GetValue(string key, Languages lang, bool logError = true)
        {
            key = key.ToLower();

            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].Key.Equals(key) && _entries[i].Language == lang)
                    return _entries[i].Value;
            }

            if (logError)
            {
                var errMsg = $"Localization failure: key[{key}] lang [{lang}] not found in [{name}];";
                Debug.LogError(errMsg);
            }

            return string.Empty;
        }
    }
}