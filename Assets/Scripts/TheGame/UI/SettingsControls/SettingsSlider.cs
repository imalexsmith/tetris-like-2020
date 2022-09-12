using System;
using UnityEngine;
using UnityEngine.UI;

// ========================
// Revision 2020.11.18
// ========================

namespace TheGame.UI
{
    [RequireComponent(typeof(Slider))]
    public class SettingsSlider : MonoBehaviour, ISettingsControl<float>
    {
        public enum SliderTargetOption
        {
            MasterVolume = 0,
            MusicVolume = 1,
            SFXVolume = 2,
            VoiceVolume = 3
        }


        // ===========================================================================================
        public SliderTargetOption TargetProperty;
        public Slider CachedSlider;


        // ===========================================================================================
        public void SetValue(float value)
        {
            switch (TargetProperty)
            {
                case SliderTargetOption.MasterVolume:
                    ApplicationSettings.Instance.MasterVolume = value;
                    break;
                case SliderTargetOption.MusicVolume:
                    ApplicationSettings.Instance.MusicVolume = value;
                    break;
                case SliderTargetOption.SFXVolume:
                    ApplicationSettings.Instance.SfxVolume = value;
                    break;
                case SliderTargetOption.VoiceVolume:
                    ApplicationSettings.Instance.VoiceVolume = value;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public float GetValue()
        {
            switch (TargetProperty)
            {
                case SliderTargetOption.MasterVolume:
                    return ApplicationSettings.Instance.MasterVolume;
                case SliderTargetOption.MusicVolume:
                    return ApplicationSettings.Instance.MusicVolume;
                case SliderTargetOption.SFXVolume:
                    return ApplicationSettings.Instance.SfxVolume;
                case SliderTargetOption.VoiceVolume:
                    return ApplicationSettings.Instance.VoiceVolume;
                default:
                    throw new NotImplementedException();
            }
        }

        protected void Reset()
        {
            CachedSlider = GetComponent<Slider>();
        }

        protected void Awake()
        {
            if (!CachedSlider)
                CachedSlider = GetComponent<Slider>();

            CachedSlider.onValueChanged.AddListener(SetValue);
        }

        protected void OnEnable()
        {
            CachedSlider.value = GetValue();
        }

        protected void OnDestroy()
        {
            CachedSlider.onValueChanged.RemoveListener(SetValue);
        }
    }
}