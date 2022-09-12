using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

// ========================
// Revision 2020.11.06
// ========================

namespace NightFramework.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlayer : MonoBehaviour
    {
        // ========================================================================================
        public RandomisedSet<AudioClip> Clips;
        public bool RandomizePitch = false;
        [ConditionalField(nameof(RandomizePitch)), Range(0f, 0.999f)]
        public float PitchRange = 0.1f;
        public AudioSource CachedAudioSource;

        private readonly Queue<AudioClip> _clipsToPlay = new Queue<AudioClip>();
        private Coroutine _playingRoutine;


        // ========================================================================================
        public void Play()
        {
            Stop();
            PlayAdditive();
        }

        public void PlayAdditive()
        {
            var clips = Clips.SelectRandomValues();
            foreach (var clip in clips)
                _clipsToPlay.Enqueue(clip);

            if (_playingRoutine == null)
                _playingRoutine = StartCoroutine(PlayingClipsQueue());
        }

        public void Stop()
        {
            CachedAudioSource.Stop();
            _clipsToPlay.Clear();
            if (_playingRoutine != null)
            {
                StopCoroutine(_playingRoutine);
                _playingRoutine = null;
            }
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

        private IEnumerator PlayingClipsQueue()
        {
            while (_clipsToPlay.Count > 0)
            {
                var clip = _clipsToPlay.Dequeue();
                var len = clip.length;

                if (RandomizePitch)
                {
                    var pitch = 1.0f + Random.Range(-PitchRange, PitchRange);
                    CachedAudioSource.pitch = pitch;
                    len /= pitch;
                }

                CachedAudioSource.PlayOneShot(clip);

                yield return new WaitForSeconds(len);
            }

            _playingRoutine = null;
        }
    }
}