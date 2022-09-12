using UnityEngine;

// ========================
// Revision 2020.11.06
// ========================

namespace NightFramework.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayAudioIfNot : MonoBehaviour
    {
        // ========================================================================================
        public AudioSource CachedAudioSource;


        // ========================================================================================
        public void Play()
        {
            if (!CachedAudioSource.isPlaying)
                CachedAudioSource.Play();
        }

        public void PlayDelayed(float delay)
        {
            if (!CachedAudioSource.isPlaying)
                CachedAudioSource.PlayDelayed(delay);
        }

        public void PlayOneShot(AudioClip clip, float volumeScale)
        {
            if (!CachedAudioSource.isPlaying)
                CachedAudioSource.PlayOneShot(clip, volumeScale);
        }

        protected void Reset()
        {
            CachedAudioSource = GetComponent<AudioSource>();
        }

        protected void Awake()
        {
            if (!CachedAudioSource)
                CachedAudioSource = GetComponent<AudioSource>();
        }
    }
}