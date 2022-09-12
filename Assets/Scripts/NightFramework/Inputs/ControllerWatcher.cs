using System.Collections;
using UnityEngine;

// ========================
// Revision 2020.11.07
// ========================

namespace NightFramework.Inputs
{
    public class ControllerWatcher : MonoBehaviour
    {
        // ========================================================================================
        public float Interval = 2f;
        public InputType FallbackType = InputType.MouseAndKeyboard;


        // ========================================================================================
        protected void Start()
        {
            StartCoroutine(Watching());
        }

        private IEnumerator Watching()
        {
            while (true)
            {
                var joysticks = Input.GetJoystickNames();
                for (int i = 0; i < joysticks.Length; i++)
                {
                    if (ApplicationSettingsBase.Instance.CurrentInputType != InputType.Controller)
                    {
                        if (!string.IsNullOrWhiteSpace(joysticks[i]))
                        {
                            ApplicationSettingsBase.Instance.CurrentInputType = InputType.Controller;
                            break;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(joysticks[i]))
                            break;

                        ApplicationSettingsBase.Instance.CurrentInputType = FallbackType;
                    }
                }

                return new WaitForSecondsRealtime(Interval);
            }
        }
    }
}