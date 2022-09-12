using UnityEngine;
using UltEvents;

// ========================
// Revision 2019.11.06
// ========================

namespace NightFramework.Inputs
{
    public class InputKeyHandler : InputHandlerBase
    {
        // ========================================================================================
        public KeyCode Key;

        [Space]
        public UltEvent OnKeyDown;
        public UltEvent WhileKeyHold;
        public UltEvent OnKeyUp;


        // ========================================================================================
        protected override void UpdateInput()
        {
            if (Key == KeyCode.None)
                return;

            if (Input.GetKeyDown(Key))
                OnKeyDown.Invoke();

            if (Input.GetKey(Key))
                WhileKeyHold.Invoke();

            if (Input.GetKeyUp(Key))
                OnKeyUp.Invoke();
        }
    }
}