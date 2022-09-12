using UnityEngine;
using UltEvents;
using NightFramework;

// ========================
// Revision 2020.11.09
// ========================

namespace TheGame
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        public enum GameplayStatus
        {
            Playing = 0,
            Defeat = 1,
            Victory = 2,
            Draw = 3
        }


        // ========================================================================================
        [SerializeField]
        private bool _isGameplayPaused;
        public bool IsGameplayPaused
        {
            get => _isGameplayPaused;
            set
            {
                if (value != _isGameplayPaused)
                {
                    _isGameplayPaused = value;

                    if (value)
                    {
                        UnpauseTimer.Stop();
                        OnGameplayPause.Invoke();
                    }
                    else
                    {
                        UnpauseTimer.Start();
                        OnGameplayUnpause.Invoke();
                    }
                }
            }
        }

        [SerializeField]
        private GameplayStatus _gameStatus;
        public GameplayStatus GameStatus
        {
            get => _gameStatus;
            set
            {
                if (value != _gameStatus)
                {
                    _gameStatus = value;

                    switch (value)
                    {
                        case GameplayStatus.Defeat:
                            DefeatTimer.Start();
                            VictoryTimer.Stop();
                            DrawTimer.Stop();
                            OnDefeat.Invoke();
                            break;
                        case GameplayStatus.Victory:
                            DefeatTimer.Stop();
                            VictoryTimer.Start();
                            DrawTimer.Stop();
                            OnVictory.Invoke();
                            break;
                        case GameplayStatus.Draw:
                            DefeatTimer.Stop();
                            VictoryTimer.Stop();
                            DrawTimer.Start();
                            OnDraw.Invoke();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        [Space]
        public UltEvent OnGameplayPause = new UltEvent();
        public UltEvent OnGameplayUnpause = new UltEvent();
        public UltEvent OnDefeat = new UltEvent();
        public UltEvent OnVictory = new UltEvent();
        public UltEvent OnDraw = new UltEvent();

        [Space]
        public SuperTimer UnpauseTimer = new SuperTimer();
        public SuperTimer DefeatTimer = new SuperTimer();
        public SuperTimer VictoryTimer = new SuperTimer();
        public SuperTimer DrawTimer = new SuperTimer();


        // ========================================================================================
        public void SwitchGameplayPause()
        {
            IsGameplayPaused = !IsGameplayPaused;
        }

        protected void Update()
        {
            UnpauseTimer.Update();
            DefeatTimer.Update();
            VictoryTimer.Update();
            DrawTimer.Update();
        }

        protected void OnApplicationPause(bool pause)
        {
            if (pause)
                IsGameplayPaused = true;
        }
    }
}