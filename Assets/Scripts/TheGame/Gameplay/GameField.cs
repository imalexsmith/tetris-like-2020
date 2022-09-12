using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;
using DG.Tweening;
using NightFramework;

// ========================
// Revision 2020.11.12
// ========================

namespace TheGame
{
    public class GameField : Singleton<GameField>
    {
        // ========================================================================================
        public const int BLOCKS_X_COUNT = 10;
        public const int BLOCKS_Y_COUNT = 22;

        public const int MIN_LEVEL = 1;
        public const int MAX_LEVEL = 30;


        // ========================================================================================
        [Header("Events")]
        public UltEvent OnReload = new UltEvent();
        public UltEvent OnNewFigure = new UltEvent();
        public UltEvent OnCantRotate = new UltEvent();
        public UltEvent OnCantShift = new UltEvent();
        public UltEvent OnInstantDrop = new UltEvent();
        public UltEvent OnPlaceOnHold = new UltEvent();
        public UltEvent OnCantPlaceOnHold = new UltEvent();
        public UltEvent OnRemoveSingleLine = new UltEvent();
        public UltEvent OnRemoveTwoLines = new UltEvent();
        public UltEvent OnRemoveThreeLines = new UltEvent();
        public UltEvent OnRemoveFourLines = new UltEvent();
        public UltEvent OnCurrentScoreChanges = new UltEvent();
        public UltEvent OnLevelUp = new UltEvent();
        public UltEvent OnDefeat = new UltEvent();

        [Header("Setup")]
        public Vector2Int StartFigurePosition = new Vector2Int(3, 17);
        [Range(0f, 1f)]
        public float LineFallTossPower = 0.26f;
        public float LineFallCameraShakePower = 0.11f;

        [Header("Move to ApplicationSettings?")]
        public float DefaultMoveTime = 0.015f;
        public float DefaultFinalizeTime = 0.55f;
        public float RepeatActionDelay = 0.152f;
        public float ShiftTime = 0.048f;
        public float SoftDownTime = 0.048f;
        public float NewFigureWaitingTime = 0.25f;
        public float RemovingBlockNextDelay = 0.035f;
        public float RemovingBlockTime = 0.25f;
        public float LineFallTime = 0.15f;
        public int SoftDropLineScore = 1;
        public int InstantDropLineScore = 2;
        public int RemoveSingleLineScore = 100;
        public int RemoveTwoLinesScore = 300;
        public int RemoveThreeLinesScore = 500;
        public int RemoveFourLinesScore = 800;
        public int LinesForLevelUp = 10;

        public Queue<FigureTypeKeys> NextFiguresSet { get; private set; } = new Queue<FigureTypeKeys>();
        public FigureTypeKeys FigureOnHold { get; private set; }
        public int CurrentLevel { get; private set; } = MIN_LEVEL;
        public float CurrentLevelMoveTime => (MAX_LEVEL + 1 - CurrentLevel) * DefaultMoveTime;
        public int CurrentScore { get; private set; } = 0;
        public int LinesRemoved { get; private set; } = 0;
        public int LinesLeftForLevelUp { get; private set; } = 0;

        private SuperTimer _moveTimer;
        private SuperTimer _finalizeTimer;
        private SuperTimer _shiftLeftDelayTimer;
        private SuperTimer _shiftLeftTimer;
        private SuperTimer _shiftRightDelayTimer;
        private SuperTimer _shiftRightTimer;
        private SuperTimer _softDownDelayTimer;
        private SuperTimer _softDownTimer;
        private SuperTimer _linesFallingTimer;
        private SuperTimer _newFigureWaitingTimer;

        private FigureBlock[] _blocks = new FigureBlock[BLOCKS_X_COUNT * BLOCKS_Y_COUNT];
        private Figure _currentFigure;
        private Figure _ghostFigure;
        private bool _onHoldPlaced;
        private Tweener _cameraShakeTween;


        // ========================================================================================
        public void Reload()
        {
            _currentFigure.Clear(true);
            _ghostFigure.Clear(true);
            _onHoldPlaced = false;

            for (var i = 0; i < _blocks.Length; i++)
            {
                if (_blocks[i] != null)
                {
                    _blocks[i].RemoveAnim();
                    _blocks[i] = null;
                }
            }

            FigureOnHold = FigureTypeKeys.None;
            CurrentLevel = MIN_LEVEL;
            CurrentScore = 0;
            LinesRemoved = 0;
            LinesLeftForLevelUp = LinesForLevelUp;

            SetupTimers();
            NextFiguresSet.Clear();
            AddFiguresToNextFiguresSet();
            NewFigure();
            Pause();
            
            OnReload.Invoke();
        }

        public void SaveCurrentScoreToStorage()
        {
            PersistentDataStorage.Instance.BestScores.Add(new BestScoresEntry { Score = CurrentScore });
            PersistentDataStorage.Instance.BestScores.Sort();
            PersistentDataStorage.Instance.BestScores.RemoveAt(PersistentDataStorage.Instance.BestScores.Count - 1);
            PersistentDataStorage.Instance.SaveToFile();
        }

        public void StopShiftLeftTimers()
        {
            _shiftLeftDelayTimer.Stop();
            _shiftLeftTimer.Stop();
        }

        public void StopShiftRightTimers()
        {
            _shiftRightDelayTimer.Stop();
            _shiftRightTimer.Stop();
        }

        public void StopSoftDownTimers()
        {
            _softDownDelayTimer.Stop();
            _softDownTimer.Stop();
        }

        public void StartFigureRotateClockwise()
        {
            _linesFallingTimer.Stop();
            if (_currentFigure.FigureType != FigureTypeKeys.None)
                _newFigureWaitingTimer.Stop();

            RotateFigure(true, true);
        }

        public void StartFigureRotateCounterClockwise()
        {
            _linesFallingTimer.Stop();
            if (_currentFigure.FigureType != FigureTypeKeys.None)
                _newFigureWaitingTimer.Stop();

            RotateFigure(false, true);
        }

        public void StartFigureShiftLeft()
        {
            _linesFallingTimer.Stop();
            if (_currentFigure.FigureType != FigureTypeKeys.None)
                _newFigureWaitingTimer.Stop();

            MoveFigureLeft();

            _shiftLeftDelayTimer.Start();
        }

        public void StartFigureShiftRight()
        {
            _linesFallingTimer.Stop();
            if (_currentFigure.FigureType != FigureTypeKeys.None)
                _newFigureWaitingTimer.Stop();

            MoveFigureRight();

            _shiftRightDelayTimer.Start();
        }

        public void StartFigureSoftDown()
        {
            _linesFallingTimer.Stop();
            if (_currentFigure.FigureType != FigureTypeKeys.None)
                _newFigureWaitingTimer.Stop();

            if (MoveFigureDown())
                AddScores(SoftDropLineScore);

            _softDownDelayTimer.Start();
        }

        public void StartInstantDrop()
        {
            _linesFallingTimer.Stop();
            if (_currentFigure.FigureType != FigureTypeKeys.None)
                _newFigureWaitingTimer.Stop();

            InstantDrop();
        }

        public void PlaceOnHold()
        {
            if (_linesFallingTimer.Status == SuperTimer.TimerStatus.Active)
                return;

            if (!_onHoldPlaced)
            {
                var t = _currentFigure.FigureType;
                _currentFigure.Clear(true);
                _ghostFigure.Clear(true);

                if (FigureOnHold != FigureTypeKeys.None)
                {
                    _currentFigure.Generate(FigureOnHold, false);
                    _currentFigure.MoveToPosition(StartFigurePosition);
                    _ghostFigure.Generate(FigureOnHold, true);
                    UpdateGhostPosition();

                    _newFigureWaitingTimer.Start();
                }
                else
                    NewFigure();

                FigureOnHold = t;
                _onHoldPlaced = true;
                OnPlaceOnHold.Invoke();
            }
            else
                OnCantPlaceOnHold.Invoke();
        }

        protected override void Awake()
        {
            base.Awake();

            LinesLeftForLevelUp = LinesForLevelUp;
        }

        protected void Start()
        {
            GameplayManager.Instance.OnGameplayPause += Pause;
            GameplayManager.Instance.UnpauseTimer.OnComplete += Unpause;

            _currentFigure = new GameObject("CurrentFigure").AddComponent<Figure>();
            _currentFigure.transform.SetParent(transform);

            _ghostFigure = new GameObject("GhostFigure").AddComponent<Figure>();
            _ghostFigure.transform.SetParent(transform);

            SetupTimers();
            AddFiguresToNextFiguresSet();
            NewFigure();
            Pause();
        }

        protected void Update()
        {
            _linesFallingTimer.Update();
            _newFigureWaitingTimer.Update();

            _moveTimer.Update();
            _finalizeTimer.Update();
            _shiftLeftDelayTimer.Update();
            _shiftLeftTimer.Update();
            _shiftRightDelayTimer.Update();
            _shiftRightTimer.Update();
            _softDownDelayTimer.Update();
            _softDownTimer.Update();
        }

        private void SetupTimers()
        {
            _moveTimer = new SuperTimer();
            _moveTimer.Duration = CurrentLevelMoveTime;
            _moveTimer.Repeatable = true;
            _moveTimer.NeedOnComplete = true;
            _moveTimer.OnComplete += () => { MoveFigureDown(); };

            _finalizeTimer = new SuperTimer();
            _finalizeTimer.Duration = Mathf.Max(DefaultFinalizeTime, CurrentLevelMoveTime + 0.001f);
            _finalizeTimer.NeedOnComplete = true;
            _finalizeTimer.OnComplete += FinalizeFigure;

            _shiftLeftDelayTimer = new SuperTimer();
            _shiftLeftDelayTimer.Duration = RepeatActionDelay;
            _shiftLeftDelayTimer.NeedOnComplete = true;
            _shiftLeftDelayTimer.OnComplete += () => { _shiftLeftTimer.Start(); };

            _shiftLeftTimer = new SuperTimer();
            _shiftLeftTimer.Duration = ShiftTime;
            _shiftLeftTimer.Repeatable = true;
            _shiftLeftTimer.NeedOnComplete = true;
            _shiftLeftTimer.OnComplete += () => { MoveFigureLeft(); };

            _shiftRightDelayTimer = new SuperTimer();
            _shiftRightDelayTimer.Duration = RepeatActionDelay;
            _shiftRightDelayTimer.NeedOnComplete = true;
            _shiftRightDelayTimer.OnComplete += () => { _shiftRightTimer.Start(); };

            _shiftRightTimer = new SuperTimer();
            _shiftRightTimer.Duration = ShiftTime;
            _shiftRightTimer.Repeatable = true;
            _shiftRightTimer.NeedOnComplete = true;
            _shiftRightTimer.OnComplete += () => { MoveFigureRight(); };

            _softDownDelayTimer = new SuperTimer();
            _softDownDelayTimer.Duration = RepeatActionDelay;
            _softDownDelayTimer.NeedOnComplete = true;
            _softDownDelayTimer.OnComplete += () => { _softDownTimer.Start(); };

            _softDownTimer = new SuperTimer();
            _softDownTimer.Duration = SoftDownTime;
            _softDownTimer.Repeatable = true;
            _softDownTimer.NeedOnComplete = true;
            _softDownTimer.OnComplete += () => { if (MoveFigureDown()) AddScores(SoftDropLineScore); };

            _linesFallingTimer = new SuperTimer();
            _linesFallingTimer.NeedOnStart = true;
            _linesFallingTimer.OnStart += () => { _moveTimer.Stop(); _finalizeTimer.Stop(); };
            _linesFallingTimer.NeedOnComplete = true;
            _linesFallingTimer.OnComplete += NewFigure;
            _linesFallingTimer.NeedOnStop = true;
            _linesFallingTimer.OnStop += () => { if (_cameraShakeTween.IsActive()) _cameraShakeTween.Complete(); };
            _linesFallingTimer.OnStop += NewFigure;

            _newFigureWaitingTimer = new SuperTimer();
            _newFigureWaitingTimer.Duration = NewFigureWaitingTime;
            _newFigureWaitingTimer.NeedOnStart = true;
            _newFigureWaitingTimer.OnStart += () => { _moveTimer.Stop(); _finalizeTimer.Stop(); };
            _newFigureWaitingTimer.NeedOnStop = true;
            _newFigureWaitingTimer.OnStop += () => { _moveTimer.Start(); _finalizeTimer.Start(); };
            _newFigureWaitingTimer.NeedOnComplete = true;
            _newFigureWaitingTimer.OnComplete += () => { _moveTimer.Start(); _finalizeTimer.Start(); };
        }

        private void Pause()
        {
            _linesFallingTimer.Pause();
            _newFigureWaitingTimer.Pause();

            _moveTimer.Pause();
            _finalizeTimer.Pause();

            StopShiftLeftTimers();
            StopShiftRightTimers();
            StopSoftDownTimers();
        }

        private void Unpause()
        {
            _linesFallingTimer.Resume();
            _newFigureWaitingTimer.Resume();

            _moveTimer.Resume();
            _finalizeTimer.Resume();
        }

        private void AddScores(int amount)
        {
            CurrentScore += amount * CurrentLevel;
            OnCurrentScoreChanges.Invoke();
        }

        // Two methods below: https://github.com/JohnnyTurbo/LD43

        /// <summary>
        /// Rotates the piece by 90 degrees in specified direction. Offest operations should almost always be attempted,
        /// unless you are rotating the piece back to its original position.
        /// </summary>
        /// <param name="clockwise">Set to true if rotating clockwise. Set to False if rotating CCW</param>
        /// <param name="shouldOffset">Set to true if offset operations should be attempted.</param>
        private void RotateFigure(bool clockwise, bool shouldOffset)
        {
            if (_currentFigure.FigureType == FigureTypeKeys.None)
                return;

            var oldRotationIndex = _currentFigure.RotationIndex;

            if (clockwise)
                _currentFigure.Rotate90();
            else
                _currentFigure.Rotate270();

            if (!shouldOffset)
                return;

            var testOffset = OffsetTests(oldRotationIndex, _currentFigure.RotationIndex);
            if (!testOffset)
            {
                RotateFigure(!clockwise, false);
                OnCantRotate.Invoke();
            }
            else
            {
                if (clockwise)
                    _ghostFigure.Rotate90();
                else
                    _ghostFigure.Rotate270();

                UpdateGhostPosition();
                _finalizeTimer.Start();
            }
        }

        /// <summary>
        /// Performs 5 tests on the piece to find a valid final location for the piece.
        /// </summary>
        /// <param name="oldRotIndex">Original rotation index of the piece</param>
        /// <param name="newRotIndex">Rotation index the piece will be rotating to</param>
        /// <returns>True if one of the tests passed and a final location was found. False if all test failed.</returns>
        private bool OffsetTests(int oldRotIndex, int newRotIndex)
        {
            Vector2Int offsetVal1, offsetVal2, endOffset;
            Vector2Int[,] curOffsetData;

            switch (_currentFigure.FigureType)
            {
                case FigureTypeKeys.O:
                    curOffsetData = Figure.O_OFFSET_DATA;
                    break;
                case FigureTypeKeys.I:
                    curOffsetData = Figure.I_OFFSET_DATA;
                    break;
                case FigureTypeKeys.J:
                case FigureTypeKeys.L:
                case FigureTypeKeys.S:
                case FigureTypeKeys.T:
                case FigureTypeKeys.Z:
                    curOffsetData = Figure.JLSTZ_OFFSET_DATA;
                    break;
                case FigureTypeKeys.None:
                default:
                    return false;
            }

            endOffset = Vector2Int.zero;

            bool movePossible = false;

            for (int testIndex = 0; testIndex < 5; testIndex++)
            {
                offsetVal1 = curOffsetData[testIndex, oldRotIndex];
                offsetVal2 = curOffsetData[testIndex, newRotIndex];
                endOffset = offsetVal1 - offsetVal2;
                if (CheckPosition(_currentFigure, _currentFigure.FigurePosition + endOffset))
                {
                    movePossible = true;
                    break;
                }
            }

            if (movePossible)
                _currentFigure.MoveToPosition(_currentFigure.FigurePosition + endOffset);

            return movePossible;
        }

        private bool MoveFigureLeft()
        {
            var newPos = _currentFigure.FigurePosition + Vector2Int.left;
            if (CheckPosition(_currentFigure, newPos))
            {
                _currentFigure.MoveToPosition(newPos);
                UpdateGhostPosition();
                _finalizeTimer.Start();

                return true;
            }
            else
            {
                OnCantShift.Invoke();
                return false;
            }
        }

        private bool MoveFigureRight()
        {
            var newPos = _currentFigure.FigurePosition + Vector2Int.right;
            if (CheckPosition(_currentFigure, newPos))
            {
                _currentFigure.MoveToPosition(newPos);
                UpdateGhostPosition();
                _finalizeTimer.Start();

                return true;
            }
            else
            {
                OnCantShift.Invoke();
                return false;
            }
        }

        private bool MoveFigureDown()
        {
            var newPos = _currentFigure.FigurePosition + Vector2Int.down;
            if (CheckPosition(_currentFigure, newPos))
            {
                _currentFigure.MoveToPosition(newPos);
                _moveTimer.Start();
                _finalizeTimer.Start();

                return true;
            }
            else
                return false;
        }

        private void InstantDrop()
        {
            var newPos = _currentFigure.FigurePosition;
            do {
                newPos += Vector2Int.down;
            } 
            while (CheckPosition(_currentFigure, newPos));

            newPos += Vector2Int.up;
            var d = _currentFigure.FigurePosition.y - newPos.y;

            _currentFigure.MoveToPosition(newPos);
            AddScores(InstantDropLineScore * d);

            FinalizeFigure();

            OnInstantDrop.Invoke();
        }

        private void FinalizeFigure()
        {
            for (int x = 0; x < Figure.LINE_BLOCKS_COUNT; x++)
            {
                for (int y = 0; y < Figure.LINE_BLOCKS_COUNT; y++)
                {
                    var iFigure = y * Figure.LINE_BLOCKS_COUNT + x;
                    if (_currentFigure.Blocks[iFigure] != null)
                    {
                        var iField = (_currentFigure.FigurePosition.y + y) * BLOCKS_X_COUNT + _currentFigure.FigurePosition.x + x;
                        if (_blocks[iField] != null)
                        {
                            Defeat();
                            return;
                        }
                        else
                        {
                            _blocks[iField] = _currentFigure.Blocks[iFigure];
                            _blocks[iField].FinalizeAnim();
                        }
                    }
                }
            }

            _currentFigure.Clear();
            _ghostFigure.Clear(true);
            _onHoldPlaced = false;

            if (!CheckLines())
                NewFigure();
        }

        private void Defeat()
        {
            SaveCurrentScoreToStorage();

            OnDefeat.Invoke();
        }

        private void NewFigure()
        {
            if (_currentFigure.FigureType != FigureTypeKeys.None)
                return;

            var figure = NextFiguresSet.Dequeue();

            _currentFigure.Generate(figure, false);
            _currentFigure.MoveToPosition(StartFigurePosition);
            _ghostFigure.Generate(figure, true);
            UpdateGhostPosition();

            if (NextFiguresSet.Count <= Enum.GetValues(typeof(FigureTypeKeys)).Length - 1)
                AddFiguresToNextFiguresSet();

            _newFigureWaitingTimer.Start();

            OnNewFigure.Invoke();
        }

        private void AddFiguresToNextFiguresSet()
        {
            var figures = Enum.GetValues(typeof(FigureTypeKeys));
            var len = figures.Length - 1;
            var rndSet = new RandomisedSet<FigureTypeKeys> {
                SelectionMode = RandomValueSelectionMode.SeveralWithNoRepeats,
                SelectionRounds = len
            };

            for (int i = 1; i < len + 1; i++)
            {
                rndSet.Values.Add(new RandomisedSetEntry<FigureTypeKeys>() 
                {
                    Value = (FigureTypeKeys)figures.GetValue(i), 
                    Weight = 1 
                });
            }

            var vals = rndSet.SelectRandomValues();
            foreach (var val in vals)
                NextFiguresSet.Enqueue(val);
        }

        private bool CheckPosition(Figure figure, Vector2Int position)
        {
            if (figure.FigureType == FigureTypeKeys.None)
                return false;

            for (int x = 0; x < Figure.LINE_BLOCKS_COUNT; x++)
            {
                for (int y = 0; y < Figure.LINE_BLOCKS_COUNT; y++)
                {
                    var iFigure = y * Figure.LINE_BLOCKS_COUNT + x;
                    if (figure.Blocks[iFigure] != null)
                    {
                        var newX = position.x + x;
                        if (newX < 0 || newX >= BLOCKS_X_COUNT)
                            return false;

                        var newY = position.y + y;
                        if (newY < 0 || newY >= BLOCKS_Y_COUNT)
                            return false;

                        var iField = newY * BLOCKS_X_COUNT + newX;
                        if (_blocks[iField] != null)
                            return false;
                    }
                }
            }

            return true;
        }

        private void UpdateGhostPosition()
        {
            var newPos = _currentFigure.FigurePosition;
            do {
                newPos += Vector2Int.down;
            }
            while (CheckPosition(_ghostFigure, newPos));

            _ghostFigure.MoveToPosition(newPos + Vector2Int.up);
        }

        private bool CheckLines()
        {
            var linesToRemove = new List<int>();
            var maxIndex = BLOCKS_Y_COUNT - 2;
            for (int y = 0; y < maxIndex; y++)
            {
                var isRemoved = true;
                for (int x = 0; x < BLOCKS_X_COUNT; x++)
                {
                    if (_blocks[y * BLOCKS_X_COUNT + x] == null)
                    {
                        isRemoved = false;
                        break;
                    }
                }
                if (isRemoved)
                {
                    linesToRemove.Add(y);
                    if (maxIndex == BLOCKS_Y_COUNT)
                        maxIndex = Mathf.Min(y + Figure.LINE_BLOCKS_COUNT, BLOCKS_Y_COUNT);
                }
            }

            if (linesToRemove.Count > 0)
            {
                RemoveLines(linesToRemove);
                return true;
            }

            return false;
        }

        private void RemoveLines(List<int> linesToRemove)
        {
            LinesRemoved += linesToRemove.Count;
            LinesLeftForLevelUp -= linesToRemove.Count;

            switch (linesToRemove.Count)
            {
                case 1:
                    AddScores(RemoveSingleLineScore);
                    OnRemoveSingleLine.Invoke();
                    break;
                case 2:
                    AddScores(RemoveTwoLinesScore);
                    OnRemoveTwoLines.Invoke();
                    break;
                case 3:
                    AddScores(RemoveThreeLinesScore);
                    OnRemoveThreeLines.Invoke();
                    break;
                case 4:
                    AddScores(RemoveFourLinesScore);
                    OnRemoveFourLines.Invoke();
                    break;
                default:
                    throw new UnityException("THIS IS IMPOSSIBLE o_O");
            }

            if (CurrentLevel < MAX_LEVEL)
                CheckLevelUp();


            var moveBlocks = new Dictionary<FigureBlock, Vector2Int>();

            for (int i = linesToRemove.Count - 1; i >= 0; i--)
            {
                var y = linesToRemove[i] * BLOCKS_X_COUNT;
                for (int x = 0; x < BLOCKS_X_COUNT; x++)
                {
                    if (_blocks[y + x] != null)
                    {
                        StartCoroutine(BlockRemoving(_blocks[y + x], x));
                        _blocks[y + x] = null;
                    }
                }

                for (int yy = linesToRemove[i] + 1; yy < BLOCKS_Y_COUNT; yy++)
                {
                    for (int xx = 0; xx < BLOCKS_X_COUNT; xx++)
                    {
                        var upper = yy * BLOCKS_X_COUNT + xx;
                        var lower = (yy - 1) * BLOCKS_X_COUNT + xx;
                        if (_blocks[upper] != null)
                        {
                            if (moveBlocks.ContainsKey(_blocks[upper]))
                                moveBlocks[_blocks[upper]] += Vector2Int.down;
                            else
                                moveBlocks.Add(_blocks[upper], new Vector2Int(xx, yy - 1));

                            _blocks[lower] = _blocks[upper];
                            _blocks[upper] = null;
                        }
                    }
                }
            }

            foreach (var block in moveBlocks)
                StartCoroutine(BlockFalling(block.Key, block.Value, linesToRemove.Count));

            var totalFallTime = (BLOCKS_X_COUNT - 1) * RemovingBlockNextDelay + RemovingBlockTime + linesToRemove.Count * LineFallTime - NewFigureWaitingTime;

            _cameraShakeTween = Camera.main.DOShakePosition(totalFallTime, linesToRemove.Count * LineFallCameraShakePower, fadeOut: false);

            _linesFallingTimer.Duration = totalFallTime;
            _linesFallingTimer.Start();
        }

        private void CheckLevelUp()
        {
            if (LinesLeftForLevelUp <= 0)
            {
                LinesLeftForLevelUp += LinesForLevelUp;
                CurrentLevel = Mathf.Min(CurrentLevel + 1, MAX_LEVEL);
                _moveTimer.Duration = CurrentLevelMoveTime;
                _finalizeTimer.Duration = Mathf.Max(DefaultFinalizeTime, CurrentLevelMoveTime + 0.001f);
                OnLevelUp.Invoke();
            }
        }

        private IEnumerator BlockRemoving(FigureBlock block, int x)
        {
            yield return new WaitForSeconds(x * RemovingBlockNextDelay);

            block.RemoveAnim();
        }

        private IEnumerator BlockFalling(FigureBlock block, Vector2Int toPos, int linesRemoved)
        {
            var delay = toPos.x * RemovingBlockNextDelay;
            var tossDur = (RemovingBlockTime + linesRemoved * LineFallTime) * LineFallTossPower;
            var fallDur = (RemovingBlockTime + linesRemoved * LineFallTime) * (1f - LineFallTossPower);
            var tossPos = block.transform.position.y + (block.transform.position.y - toPos.y) * LineFallTossPower;

            yield return new WaitForSeconds(delay);

            block.transform.DOMoveY(tossPos, tossDur);
            yield return new WaitForSeconds(tossDur);

            block.transform.DOMoveY(toPos.y, fallDur);
            yield return new WaitForSeconds(fallDur);

            block.transform.position = new Vector3(toPos.x, toPos.y, 0f);
        }
    }
}