/*using UnityEngine;
using UnityEngine.EventSystems;

// ========================
// Revision 2020.03.02
// ========================

namespace NightFramework.UI
{
    public abstract class FloatingText<T> : MonoBehaviour where T : UIBehaviour
    {
       *//* // ========================================================================================
        public float Duration = 1f;

        [SerializeField]
        private T _cachedText;
        public T CachedText
        {
            get => _cachedText;
            protected set => _cachedText = value;
        }

        public ProgressiveTimer Timer { get; private set; }


        // ========================================================================================
        public override void WakeUp()
        {
            base.WakeUp();

            Timer.Duration = Duration;
            Timer.Start();
        }

        protected void Awake()
        {
            if (!_cachedText)
                _cachedText = GetComponent<T>();

            Timer = new ProgressiveTimer();
            Timer.OnComplete += Free;
        }

        protected void Update()
        {
            Timer.Update();
        }*//*
    }
}
*/