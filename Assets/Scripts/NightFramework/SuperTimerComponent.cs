using System;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

// ========================
// Revision 2020.11.09
// ========================

namespace NightFramework
{
    public sealed class SuperTimerComponent : MonoBehaviour
    {
        public SuperTimer Timer;

        private void Update()
        {
            Timer.Update();
        }
    }


    [Serializable]
    public class SuperTimer
    {
        public enum TimerStatus
        {
            Inactive = 0,
            Active = 1,
            Pause = 2,
            Comlete = 3
        }

        // ========================================================================================
        public bool NeedOnStart;
        public UltEvent OnStart = new UltEvent();

        public bool NeedOnPause;
        public UltEvent OnPause = new UltEvent();

        public bool NeedOnResume;
        public UltEvent OnResume = new UltEvent();

        public bool NeedOnStop;
        public UltEvent OnStop = new UltEvent();

        public bool NeedOnComplete = true;
        public UltEvent OnComplete = new UltEvent();

        public float Duration;
        public bool Repeatable;
        public List<SuperTimer> SubTimers = new List<SuperTimer>();

        public float Progress => _timeCounter / Duration;

        public int NumberOfCompletions { get; private set; }

        public TimerStatus Status { get; private set; } = TimerStatus.Inactive;

        private float _timeCounter;
        private float _lastUpdate;


        // ========================================================================================
        public void Start()
        {
            _timeCounter = 0f;
            _lastUpdate = Time.time;
            NumberOfCompletions = 0;

            Status = TimerStatus.Active;

            SubTimers.ForEach(x => x.Start());

            if (NeedOnStart)
                OnStart.Invoke();
        }

        public void Pause()
        {
            if (Status != TimerStatus.Active)
                return;

            UpdateProgress();

            Status = TimerStatus.Pause;

            SubTimers.ForEach(x => x.Pause());

            if (NeedOnPause)
                OnPause.Invoke();
        }

        public void Resume()
        {
            if (Status != TimerStatus.Pause)
                return;

            _lastUpdate = Time.time;

            Status = TimerStatus.Active;

            SubTimers.ForEach(x => x.Resume());

            if (NeedOnResume)
                OnResume.Invoke();
        }

        public void Stop()
        {
            if (Status == TimerStatus.Inactive)
                return;

            _timeCounter = 0f;
            NumberOfCompletions = 0;

            Status = TimerStatus.Inactive;

            SubTimers.ForEach(x => x.Stop());

            if (NeedOnStop)
                OnStop.Invoke();
        }

        public void Update()
        {
            if (Status != TimerStatus.Active)
                return;

            SubTimers.ForEach(x => x.Update());

            UpdateProgress();
        }

        private void UpdateProgress()
        {
            _timeCounter += Time.time - _lastUpdate;
            _lastUpdate = Time.time;

            if (_timeCounter > Duration)
            {
                NumberOfCompletions++;

                if (Repeatable)
                {
                    _timeCounter -= Duration;
                }
                else
                {
                    _timeCounter = Duration;
                    Status = TimerStatus.Comlete;
                }

                if (NeedOnComplete)
                    OnComplete.Invoke();
            }
        }
    }
}