using UnityEngine;
using TMPro;

// ========================
// Revision 2020.11.17
// ========================

namespace NightFramework.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class SliderValueToTextTMP : SliderValueToText, ITMPTextHolder
    {
        [field: SerializeField]
        public TMP_Text CachedText { get; set; }
        public override string Text { get => CachedText.text; set => CachedText.text = value; }
    }
}