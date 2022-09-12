using UnityEngine;
using UltEvents;

// ========================
// Revision 2020.04.05
// ========================

namespace NightFramework.Inputs
{
    [System.Serializable]
    public sealed class InputAxisEvent1 : UltEvent<float> { };
    [System.Serializable]
    public sealed class InputAxisEvent2 : UltEvent<float, float> { };

    public class InputAxisHandler : InputHandlerBase
    {
        // ========================================================================================
        public string AxisName;

        [Space]
        public InputAxisEvent2 Value;

        public InputAxisEvent2 OnValueChange;

        public InputAxisEvent2 OnAxisStart;
        public InputAxisEvent2 OnAxisStop;

        public InputAxisEvent2 WhileAxisPositiveHold;
        public InputAxisEvent2 WhileAxisNegativeHold;

        private float _lastValue, _currentValue;


        // ========================================================================================
        protected override void UpdateInput()
        {
            if (string.IsNullOrWhiteSpace(AxisName))
                return;

            _currentValue = Input.GetAxis(AxisName);

            Value.Invoke(_lastValue, _currentValue);

            if (_currentValue != _lastValue)
                OnValueChange.Invoke(_lastValue, _currentValue);

            switch (_currentValue)
            {
                case var curval when curval == 0f:
                {
                    if (_lastValue != 0)
                        OnAxisStop.Invoke(_lastValue, _currentValue);
                }
                break;
                case var curval when curval > 0f:
                {
                    if (_lastValue == 0f)
                        OnAxisStart.Invoke(_lastValue, _currentValue);

                    WhileAxisPositiveHold.Invoke(_lastValue, _currentValue);
                }
                break;
                case var curval when curval < 0f:
                {
                    if (_lastValue == 0f)
                        OnAxisStart.Invoke(_lastValue, _currentValue);

                    WhileAxisNegativeHold.Invoke(_lastValue, _currentValue);
                }
                break;
            }

            _lastValue = _currentValue;
        }
    }
}