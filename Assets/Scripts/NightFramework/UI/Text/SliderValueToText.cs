using UnityEngine.UI;

// ========================
// Revision 2020.11.17
// ========================

namespace NightFramework.UI
{
    public abstract class SliderValueToText : AdaptiveTextHolder
    {
        // ===========================================================================================
        public Slider Source;


        // ===========================================================================================
        protected override void Awake()
        {
            base.Awake();

            Source.onValueChanged.AddListener(SetValue);
        }

        protected void OnEnable()
        {
            SetValue(Source.value);
        }

        protected void OnDestroy()
        {
            Source.onValueChanged.RemoveListener(SetValue);
        }

        private void SetValue(float value)
        {
            Text = value.ToString();
        }
    }
}