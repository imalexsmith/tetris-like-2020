using System;
using UnityEngine;
using UnityEngine.UI;

// ========================
// Revision 2020.11.18
// ========================

namespace TheGame.UI
{
    [RequireComponent(typeof(Toggle))]
    public class SettingsToggle : MonoBehaviour, ISettingsControl<bool>
    {
        public enum ToggleTargetOption
        {
            ShowFPS = 0,
            AnimMenus = 1,
            Postprocessing = 2
        }


        // ===========================================================================================
        public ToggleTargetOption TargetProperty;
        public Toggle CachedToggle;


        // ===========================================================================================
        public void SetValue(bool value)
        {
            switch (TargetProperty)
            {
                case ToggleTargetOption.ShowFPS:
                    ApplicationSettings.Instance.ShowFps = value;
                    break;
                case ToggleTargetOption.AnimMenus:
                    ApplicationSettings.Instance.AnimMenus = value;
                    break;
                case ToggleTargetOption.Postprocessing:
                    ApplicationSettings.Instance.PostProcessing = value;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public bool GetValue()
        {
            switch (TargetProperty)
            {
                case ToggleTargetOption.ShowFPS:
                    return ApplicationSettings.Instance.ShowFps;
                case ToggleTargetOption.AnimMenus:
                    return ApplicationSettings.Instance.AnimMenus;
                case ToggleTargetOption.Postprocessing:
                    return ApplicationSettings.Instance.PostProcessing;
                default:
                    throw new NotImplementedException();
            }
        }

        protected void Reset()
        {
            CachedToggle = GetComponent<Toggle>();
        }

        protected void Awake()
        {
            if (!CachedToggle)
                CachedToggle = GetComponent<Toggle>();

            CachedToggle.onValueChanged.AddListener(SetValue);
        }

        protected void OnEnable()
        {
            CachedToggle.isOn = GetValue();
        }

        protected void OnDestroy()
        {
            CachedToggle.onValueChanged.RemoveListener(SetValue);
        }
    }
}