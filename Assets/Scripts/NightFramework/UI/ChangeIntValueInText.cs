using UnityEngine;
using UnityEngine.UI;


namespace NightFramework.UI
{
    public class ChangeIntValueInText : MonoBehaviour
    {
        // ========================================================================================
        [Tooltip("Set only ONE property - Input or Text!")]
        public InputField TargetInput;
        [Tooltip("Set only ONE property - Input or Text!")]
        public Text TargetText;


        // ========================================================================================
        public void SetValue(int value)
        {
            if (TargetInput != null)
            {
                SetValueInput(value);
                return;
            }

            if (TargetText != null)
            {
                SetValueText(value);
            }
        }

        public void PlusOne()
        {
            if (TargetInput != null)
            {
                int v;
                if (int.TryParse(TargetInput.text, out v))
                    SetValueInput(v + 1);
                return;
            }

            if (TargetText != null)
            {
                int v;
                if (int.TryParse(TargetInput.text, out v))
                    SetValueText(v + 1);
            }
        }

        public void MinusOne()
        {
            if (TargetInput != null)
            {
                int v;
                if (int.TryParse(TargetInput.text, out v))
                    SetValueInput(v - 1);
                return;
            }

            if (TargetText != null)
            {
                int v;
                if (int.TryParse(TargetInput.text, out v))
                    SetValueText(v - 1);
            }
        }

        private void SetValueInput(int value)
        {
            if (TargetInput != null)
                TargetInput.text = value.ToString();
        }

        private void SetValueText(int value)
        {
            if (TargetText != null)
                TargetText.text = value.ToString();
        }
    }
}