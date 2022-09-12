using UnityEngine;
using UltEvents;

// ========================
// Revision 2019.11.06
// ========================

namespace NightFramework.Inputs
{
    public class InputButtonHandler : InputHandlerBase
    {
        // ========================================================================================
        public string ButtonName;

        [Space]
        public UltEvent OnButtonDown;
        public UltEvent WhileButtonHold;
        public UltEvent OnButtonUp;


        // ========================================================================================
        protected override void UpdateInput()
        {
            if (string.IsNullOrWhiteSpace(ButtonName))
                return;

            if (Input.GetButtonDown(ButtonName))
                OnButtonDown.Invoke();

            if (Input.GetButton(ButtonName))
                WhileButtonHold.Invoke();

            if (Input.GetButtonUp(ButtonName))
                OnButtonUp.Invoke();
        }
    }
}