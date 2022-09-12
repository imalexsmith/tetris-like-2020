using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NightFramework;

// ========================
// Revision 2020.11.18
// ========================

namespace TheGame.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LanguageDropdown : MonoBehaviour, ISettingsControl<int>
    {
        [Serializable]
        public struct LanguageDropdownItem
        {
            public Localization.Languages Language;
            public string Text;
            public Sprite Image;

            public static implicit operator TMP_Dropdown.OptionData(LanguageDropdownItem obj)
            {
                return new TMP_Dropdown.OptionData(obj.Text, obj.Image);
            }
        }


        // ========================================================================================
        public List<LanguageDropdownItem> LanguagesList;
        public TMP_Dropdown CachedDropdown;


        // ========================================================================================     
        public void SetValue(int value)
        {
            ApplicationSettings.Instance.Language = LanguagesList[value].Language;
        }

        public int GetValue()
        {
            for (int i = 0; i < LanguagesList.Count; i++)
            {
                if (LanguagesList[i].Language == ApplicationSettings.Instance.Language)
                    return i;
            }

            return 0;
        }

        protected void Reset()
        {
            LanguagesList = new List<LanguageDropdownItem>();

            var names = Enum.GetNames(typeof(Localization.Languages));
            for (int i = 0; i < names.Length; i++)
            {
                LanguagesList.Add(new LanguageDropdownItem {
                    Language = (Localization.Languages)i, 
                    Text = names[i] 
                });
            }

            CachedDropdown = GetComponent<TMP_Dropdown>();
        }

        protected void Awake()
        {
            if (!CachedDropdown)
                CachedDropdown = GetComponent<TMP_Dropdown>();

            CachedDropdown.options.Clear();
            for (int i = 0; i < LanguagesList.Count; i++)
                CachedDropdown.options.Add(LanguagesList[i]);

            CachedDropdown.onValueChanged.AddListener(SetValue);
        }

        protected void OnEnable()
        {
            CachedDropdown.value = GetValue();
        }

        protected void OnDestroy()
        {
            CachedDropdown.onValueChanged.RemoveListener(SetValue);
        }
    }
}