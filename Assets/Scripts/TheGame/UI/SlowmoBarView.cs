using UnityEngine;
using UnityEngine.UI;

// ========================
// Revision 2020.11.03
// ========================

namespace TheGame.UI
{
    public class SlowmoBarView : MonoBehaviour
    {
        // ========================================================================================
        public Sprite LessThresholdSprite;
        public Sprite MoreThresholdSprite;
        public Slider TargetSlider;
        public Image FillImage;


        // ========================================================================================
        protected void LateUpdate()
        {
            TargetSlider.value = Slowmo.Instance.CurrentAmount;

            if (Slowmo.Instance.CurrentAmount < Slowmo.Instance.Threshold && !Slowmo.Instance.IsInSlowmo)
                FillImage.sprite = LessThresholdSprite;
            else
                FillImage.sprite = MoreThresholdSprite;
        }
    }
}