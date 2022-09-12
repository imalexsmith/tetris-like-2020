using UnityEngine;
using TMPro;

// ========================
// Revision 2020.03.02
// ========================

namespace NightFramework.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextTyperTMP : TextTyper<TMP_Text>
    {
        protected override string GetText()
        {
            return CachedText.text;
        }

        protected override void SetText(string text)
        {
            CachedText.text = text;
        }
    }
}