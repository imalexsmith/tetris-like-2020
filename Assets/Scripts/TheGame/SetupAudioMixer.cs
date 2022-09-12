using UnityEngine;
using UnityEngine.Audio;

// ========================
// Revision 2020.10.22
// ========================

namespace TheGame
{
    public class SetupAudioMixer : MonoBehaviour
    {
        // ========================================================================================  
        public AudioMixer Mixer;


        // ======================================================================================== 
        protected void Awake()
        {
            ApplicationSettings.Instance.OnMasterVolumeChange += SetMasterVolume;
            ApplicationSettings.Instance.OnMusicVolumeChange += SetMusicVolume;
            ApplicationSettings.Instance.OnSfxVolumeChange += SetSfxVolume;
            ApplicationSettings.Instance.OnVoiceVolumeChange += SetVoiceVolume;
        }

        protected void Start()
        {
            SetMasterVolume(0f, ApplicationSettings.Instance.MasterVolume);
            SetMusicVolume(0f, ApplicationSettings.Instance.MusicVolume);
            SetSfxVolume(0f, ApplicationSettings.Instance.SfxVolume);
            SetVoiceVolume(0f, ApplicationSettings.Instance.VoiceVolume);
        }

        protected void OnDestroy()
        {
            ApplicationSettings.Instance.OnMasterVolumeChange -= SetMasterVolume;
            ApplicationSettings.Instance.OnMusicVolumeChange -= SetMusicVolume;
            ApplicationSettings.Instance.OnSfxVolumeChange -= SetSfxVolume;
            ApplicationSettings.Instance.OnVoiceVolumeChange -= SetVoiceVolume;
        }

        private void SetMasterVolume(float oldVal, float newVal)
        {
            if (Mixer != null)
                Mixer.SetFloat("MasterVolume", Mathf.Log10(newVal) * 20f);
        }

        private void SetMusicVolume(float oldVal, float newVal)
        {
            if (Mixer != null)
                Mixer.SetFloat("MusicVolume", Mathf.Log10(newVal) * 20f);
        }

        private void SetSfxVolume(float oldVal, float newVal)
        {
            if (Mixer != null)
                Mixer.SetFloat("SFXVolume", Mathf.Log10(newVal) * 20f);
        }

        private void SetVoiceVolume(float oldVal, float newVal)
        {
            if (Mixer != null)
                Mixer.SetFloat("VoiceVolume", Mathf.Log10(newVal) * 20f);
        }
    }
}
