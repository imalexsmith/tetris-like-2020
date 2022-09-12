using System.Globalization;
using UnityEngine;
using TMPro;

// ========================
// Revision 2020.11.13
// ========================

namespace TheGame.UI
{
    public class BestScoreValueView : MonoBehaviour
    {
        // ========================================================================================
        private static readonly NumberFormatInfo formatInfo = new NumberFormatInfo {
            NumberGroupSeparator = "\u2009", 
            NumberDecimalDigits = 0 
        };


        // ========================================================================================
        public GameObject[] EnableIfCurrent;
        public Color[] PaintIfCurrent = new Color[4];
        public TextMeshProUGUI ValueText;


        // ========================================================================================
        public void SetValue(int value, bool isCurrent)
        {
            if (isCurrent)
            {
                ValueText.enableVertexGradient = true;
                ValueText.colorGradient = new VertexGradient(PaintIfCurrent[0], PaintIfCurrent[1], PaintIfCurrent[2], PaintIfCurrent[3]);
                foreach (var go in EnableIfCurrent)
                    go.SetActive(true);
            }
            else
            {
                ValueText.enableVertexGradient = false;
                foreach (var go in EnableIfCurrent)
                    go.SetActive(false);
            }

            ValueText.text = value.ToString("n", formatInfo);
        }
    }
}