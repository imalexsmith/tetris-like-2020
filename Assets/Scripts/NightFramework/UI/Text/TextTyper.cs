using UnityEngine;
using UnityEngine.EventSystems;
using UltEvents;

// ========================
// Revision 2020.03.02
// ========================

namespace NightFramework.UI
{
    [DisallowMultipleComponent]
    public abstract class TextTyper<T> : MonoBehaviour where T : UIBehaviour
    {
        // ========================================================================================
        public UltEvent OnTypeStart = new UltEvent();
        public UltEvent OnSymbolTyped = new UltEvent();
        public UltEvent OnTypeComplete = new UltEvent();

        [Space]
        public bool AutoResetOnEnable = true;
        public float TypeStartDelay;
        public float SymbolPause = 0.0133f;

        [SerializeField]
        private T _cachedText;
        public T CachedText
        {
            get => _cachedText;
            protected set => _cachedText = value;
        }

        public bool Pause { get; set; }
        public string Message { get; private set; }
        public float ApproximateTime { get; private set; }

        private float _startDelayTimer;
        private float _symbolTypeTimer;
        private int _symbolIndex;
        private bool _typeCompleted;


        // ========================================================================================
        public void ResetTyper()
        {
            Message = GetText();
            SetText("");
            _startDelayTimer = 0f;
            _symbolTypeTimer = 0f;
            _symbolIndex = 0;
            _typeCompleted = false;
            Pause = false;
            ApproximateTime = string.IsNullOrEmpty(Message) ? TypeStartDelay : TypeStartDelay + Message.Length * SymbolPause;
        }

        public void TypeAllImmediately()
        {
            SetText(Message);
            _symbolIndex = Message.Length;
            FinalizeTyping();
        }

        protected abstract string GetText();

        protected abstract void SetText(string text);

        protected void Awake()
        {
            if (!_cachedText)
                _cachedText = GetComponent<T>();
        }

        protected void OnEnable()
        {
            if (AutoResetOnEnable)
                ResetTyper();
        }

        protected void Update()
        {
            if (_typeCompleted || Pause || string.IsNullOrEmpty(Message))
                return;

            if (TypeStartDelay > 0 && _startDelayTimer < TypeStartDelay)
            {
                _startDelayTimer += Time.deltaTime;
                return;
            }

            _symbolTypeTimer += Time.deltaTime;
            if (_symbolTypeTimer >= SymbolPause)
            {
                if (_symbolIndex == 0)
                    OnTypeStart.Invoke();

                SetText(GetText() + Message[_symbolIndex]);
                _symbolIndex++;
                _symbolTypeTimer -= SymbolPause;

                OnSymbolTyped.Invoke();

                if (_symbolIndex >= Message.Length)
                    FinalizeTyping();
            }
        }

        private void FinalizeTyping()
        {
            _typeCompleted = true;
            OnTypeComplete.Invoke();
        }
    }
}