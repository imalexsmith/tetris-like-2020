using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using NightFramework;
using DG.Tweening;

// ========================
// Revision 2020.12.16
// ========================

namespace TheGame
{
    public class Slowmo : Singleton<Slowmo>
    {
        // ========================================================================================
        public Volume BloomVolume;
        public AudioMixer Mixer;

        [Header("Move to ApplicationSettings?")]
        public float MaxDuration = 10f;
        public float FullRefillTime = 40f;
        public float SmoothingTime = 0.55f;
        [Range(0f, 1f)]
        public float Threshold = 0.25f;
        [Range(0f, 1f)]
        public float SlowGameplayFactor = 0.15f;
        [Range(0f, 1f)]
        public float SlowAudioFactor = 0.875f;

        public bool IsInSlowmo { get; private set; }
        public float CurrentAmount { get; private set; } = 1f;

        private Tweener _timeScaleTween;
        private Tweener _bloomTween;
        private Tweener _audioMixerTween;


        // ========================================================================================
        public void StartSlowmo()
        {
            if (CurrentAmount < Threshold)
                return;

            IsInSlowmo = true;

            if (_timeScaleTween.IsActive())
                _timeScaleTween.Kill();

            _timeScaleTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, SlowGameplayFactor, SmoothingTime);

            if (_bloomTween.IsActive())
                _bloomTween.Kill();

            _bloomTween = DOTween.To(() => BloomVolume.weight, x => BloomVolume.weight = x, 1f, SmoothingTime);

            if (_audioMixerTween.IsActive())
                _audioMixerTween.Kill();

            _audioMixerTween = Mixer.DOSetFloat("MasterPitch", SlowAudioFactor, SmoothingTime);
        }

        public void StopSlowmo()
        {
            IsInSlowmo = false;

            if (_timeScaleTween.IsActive())
                _timeScaleTween.Kill();

            _timeScaleTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, SmoothingTime);

            if (_bloomTween.IsActive())
                _bloomTween.Kill();

            _bloomTween = DOTween.To(() => BloomVolume.weight, x => BloomVolume.weight = x, 0f, SmoothingTime);

            if (_audioMixerTween.IsActive())
                _audioMixerTween.Kill();

            _audioMixerTween = Mixer.DOSetFloat("MasterPitch", 1f, SmoothingTime);
        }

        protected void Start()
        {
            GameplayManager.Instance.OnGameplayPause += StopSlowmo;
            GameField.Instance.OnReload += () => { 
                StopSlowmo(); 
                CurrentAmount = 1f;
            };
        }

        protected void Update()
        {
            if (GameplayManager.Instance.IsGameplayPaused || GameplayManager.Instance.UnpauseTimer.Status == SuperTimer.TimerStatus.Active)
                return;

            if (IsInSlowmo)
            {
                CurrentAmount = Mathf.Clamp(CurrentAmount - Time.unscaledDeltaTime / MaxDuration, 0f, 1f);
                if (CurrentAmount == 0f)
                    StopSlowmo();
            }
            else
            {
                CurrentAmount = Mathf.Clamp(CurrentAmount + Time.unscaledDeltaTime / FullRefillTime, 0f, 1f);
            }
        }
    }
}