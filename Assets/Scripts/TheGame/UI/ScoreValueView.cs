using UnityEngine;
using TMPro;
using Coffee.UIEffects;

// ========================
// Revision 2020.11.03
// ========================

namespace TheGame.UI
{
    public class ScoreValueView : MonoBehaviour
    {
        // ========================================================================================
        public UIShiny BorderEffect;
        public TextMeshProUGUI ValueText;


        // ========================================================================================
        public void RefreshView()
        {
            ValueText.text = GameField.Instance.CurrentScore.ToString();
            if (BorderEffect)
                BorderEffect.Play();
        }

        protected void Start()
        {
            ValueText.text = GameField.Instance.CurrentScore.ToString();

            GameField.Instance.OnCurrentScoreChanges += RefreshView;
            GameField.Instance.OnReload += RefreshView;
        }
    }
}