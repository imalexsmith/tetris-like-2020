using UnityEngine;
using UnityEngine.UI;


namespace NightFramework.UI
{
    [RequireComponent(typeof(Text))]
    public class AnimatedIntCountDrawer : MonoBehaviour
    {
        // ===========================================================================================
        public int TargetValue;
        public float AnimationDelay;
        [Tooltip("How many seconds will it take to switch to the next value")]
        public float AnimationSpeed = 1f;

        private Text _text;
        private int _currentValue;
        private float _totalTime;
        private float _currentStepTime;


        // ===========================================================================================
        public void Play()
        {
            _currentValue = 0;
            enabled = true;
        }

        public void Play(int targetValue)
        {
            TargetValue = targetValue;

            Play();
        }

        public void Play(int targetValue, float delay)
        {
            AnimationDelay = delay;

            Play(targetValue);
        }

        public void Play(int targetValue, float delay, float speed)
        {
            AnimationSpeed = speed;

            Play(targetValue, delay);
        }

        protected void Awake()
        {
            _text = GetComponent<Text>();
        }

        protected void OnEnable()
        {
            if (_text != null)
                _text.text = _currentValue.ToString();

            _totalTime = 0f;
            _currentStepTime = 0f;
        }

        protected void Update()
        {
            _totalTime += Time.deltaTime;
            
            if (_totalTime < AnimationDelay)
                return;

            if (_currentStepTime >= AnimationSpeed)
            {
                var addVal = (int)(_currentStepTime / AnimationSpeed);
                _currentStepTime = _currentStepTime % AnimationSpeed;

                _currentValue = Mathf.Min(_currentValue + addVal, TargetValue);
                _text.text = _currentValue.ToString();

                if (_currentValue >= TargetValue)
                    enabled = false;
            }
            else
                _currentStepTime += Time.deltaTime;
        }
    }
}
