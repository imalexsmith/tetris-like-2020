using UnityEngine;
using TMPro;
using Coffee.UIEffects;

// ========================
// Revision 2020.11.03
// ========================

namespace TheGame.UI
{
    public class LevelValueView : MonoBehaviour
    {
        // ========================================================================================
        public UIShiny BorderEffect;
        public TextMeshProUGUI ValueText;


        // ========================================================================================
        public void RefreshView()
        {
            ValueText.text = ValuableText();
            BorderEffect.Play();
        }

        protected void Start()
        {
            ValueText.text = ValuableText();

            GameField.Instance.OnLevelUp += RefreshView;
            GameField.Instance.OnReload += RefreshView;
        }

        private string ValuableText()
        {
            return GameField.Instance.CurrentLevel < GameField.MAX_LEVEL
                    ? $"{GameField.Instance.CurrentLevel}/{GameField.MAX_LEVEL}"
                    : "MAX";
        }
    }
}