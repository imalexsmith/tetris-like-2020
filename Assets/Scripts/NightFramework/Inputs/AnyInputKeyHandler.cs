using UnityEngine;
using UltEvents;

// ========================
// Revision 2020.10.30
// ========================

namespace NightFramework.Inputs
{
    public class AnyInputKeyHandler : InputHandlerBase
    {
        // ========================================================================================
        [Space]
        public UltEvent OnAnyKeyDown;
        public UltEvent WhileAnyKeyHold;


        // ========================================================================================
        protected override void UpdateInput()
        {
            if (Input.anyKeyDown)
                OnAnyKeyDown.Invoke();

            if (Input.anyKey)
                WhileAnyKeyHold.Invoke();
        }
    }
}