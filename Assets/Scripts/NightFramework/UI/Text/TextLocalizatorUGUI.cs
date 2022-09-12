using UnityEngine;
using UnityEngine.UI;

// ========================
// Revision 2020.03.02
// ========================

namespace NightFramework.UI
{
    [RequireComponent(typeof(Text))]
    public class TextLocalizatorUGUI : TextLocalizator<Text>
    {
        protected override void SetText(string text)
        {
            CachedText.text = text;
        }
    }
}
