using UnityEngine;
using UnityEngine.UI;


namespace NightFramework.UI
{
    [RequireComponent(typeof (Graphic)), DisallowMultipleComponent]
    public class GraphicColorSwitcher : MonoBehaviour
    {
        // ===========================================================================================
        public Color SwitchedOnColor = Color.green;
        public Color SwitchedOffColor = Color.red;

        [SerializeField]
        private bool _switchedOn = true;
        public bool SwitchedOn
        {
            get => _switchedOn;
            set
            {
                _switchedOn = value;

                SetGraphicColor(_switchedOn);
            }
        }

        private Graphic _graphic;


        // ===========================================================================================
        public virtual void Switch()
        {
            SwitchedOn = !SwitchedOn;
        }

        public virtual void Switch(bool isOn)
        {
            SwitchedOn = isOn;
        }

        protected void Awake()
        {
            _graphic = GetComponent<Graphic>();
        }

        protected void Start()
        {
            SetGraphicColor(_switchedOn);
        }

        private void SetGraphicColor(bool isOn)
        {
            var col = isOn ? SwitchedOnColor : SwitchedOffColor;

            if (_graphic != null)
                _graphic.color = col;
        }
    }
}