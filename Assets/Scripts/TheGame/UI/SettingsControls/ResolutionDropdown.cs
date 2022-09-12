using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ========================
// Revision 2020.11.18
// ========================

namespace TheGame.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class ResolutionDropdown : MonoBehaviour, ISettingsControl<int>
    {
        [Serializable]
        public struct ResolutionDropdownItem
        {
            public int Width;
            public int Height;
            public string Text;
            public Sprite Image;

            public static implicit operator TMP_Dropdown.OptionData(ResolutionDropdownItem obj)
            {
                return new TMP_Dropdown.OptionData(obj.Text, obj.Image);
            }
        }


        // ========================================================================================
        public List<ResolutionDropdownItem> ResolutionsList;
        public TMP_Dropdown CachedDropdown;


        // ========================================================================================     
        public void SetValue(int value)
        {
            var width = ResolutionsList[value].Width;
            var height = ResolutionsList[value].Height;
            if (width != Screen.width || height != Screen.height)
                Screen.SetResolution(width, height, Screen.fullScreen);
        }

        public int GetValue()
        {
            for (var i = 0; i < ResolutionsList.Count; i++)
            {
                if (ResolutionsList[i].Width == Screen.width && ResolutionsList[i].Height == Screen.height)
                    return i;
            }

            return 0;
        }

        protected void Reset()
        {
            ResolutionsList = new List<ResolutionDropdownItem>
            {
                new ResolutionDropdownItem { Width = 800, Height = 600, Text = "800*600" },
                new ResolutionDropdownItem { Width = 1024, Height = 768, Text = "1024*768" },
                new ResolutionDropdownItem { Width = 1280, Height = 720, Text = "1280*720" },
                new ResolutionDropdownItem { Width = 1280, Height = 800, Text = "1280*800" },
                new ResolutionDropdownItem { Width = 1280, Height = 1024, Text = "1280*1024" },
                new ResolutionDropdownItem { Width = 1360, Height = 768, Text = "1360*768" },
                new ResolutionDropdownItem { Width = 1366, Height = 768, Text = "1366*768" },
                new ResolutionDropdownItem { Width = 1440, Height = 900, Text = "1440*900" },
                new ResolutionDropdownItem { Width = 1600, Height = 900, Text = "1600*900" },
                new ResolutionDropdownItem { Width = 1680, Height = 1050, Text = "1680*1050" },
                new ResolutionDropdownItem { Width = 1920, Height = 1080, Text = "1920*1080" },
                new ResolutionDropdownItem { Width = 1920, Height = 1200, Text = "1920*1200" },
                new ResolutionDropdownItem { Width = 2560, Height = 1080, Text = "2560*1080" },
                new ResolutionDropdownItem { Width = 2560, Height = 1440, Text = "2560*1440" }
            };

            CachedDropdown = GetComponent<TMP_Dropdown>();
        }

        protected void Awake()
        {
            if (!CachedDropdown)
                CachedDropdown = GetComponent<TMP_Dropdown>();

            CachedDropdown.options.Clear();
            for (int i = 0; i < ResolutionsList.Count; i++)
                CachedDropdown.options.Add(ResolutionsList[i]);

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