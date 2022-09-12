using UnityEngine;

// ========================
// Revision 2020.11.12
// ========================

namespace NightFramework.Inputs
{
    public abstract class InputHandlerBase : MonoBehaviour
    {
        // ========================================================================================
        public InputHandlersGroup HandlersGroup;
        public bool SearchGroupInParentOnStart = true;
        public InputType InputDevice;

        protected bool InputDeviceIsSelectedInSettings { get; private set; }


        // ========================================================================================
        protected void Awake()
        {
            if (HandlersGroup != null)
                HandlersGroup.RegisterHandler(this);

            ApplicationSettingsBase.Instance.OnCurrentInputTypeChange += OnCurrentInputTypeChange;
        }

        protected void Start()
        {
            if (SearchGroupInParentOnStart)
            {
                HandlersGroup = GetComponentInParent<InputHandlersGroup>();

                if (HandlersGroup != null)
                    HandlersGroup.RegisterHandler(this);
            }

            InputDeviceIsSelectedInSettings = CheckDevice(ApplicationSettingsBase.Instance.CurrentInputType);
        }

        protected void Update()
        {
            if (!InputDeviceIsSelectedInSettings)
                return;

            UpdateInput();
        }

        protected void OnDestroy()
        {
            if (HandlersGroup != null)
                HandlersGroup.UnregisterHandler(this);

            HandlersGroup = null;

            ApplicationSettingsBase.Instance.OnCurrentInputTypeChange -= OnCurrentInputTypeChange;
        }

        protected abstract void UpdateInput();

        private void OnCurrentInputTypeChange(InputType oldVal, InputType newVal)
        {
            InputDeviceIsSelectedInSettings = CheckDevice(newVal); 
        }

        private bool CheckDevice(InputType val)
        {
            return (InputDevice == InputType.Undefined || val == InputType.Undefined || InputDevice == val);
        }
    }
}