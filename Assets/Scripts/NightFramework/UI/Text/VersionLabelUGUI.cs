using UnityEngine;
using UnityEngine.UI;

// ========================
// Revision 2020.11.17
// ========================

namespace NightFramework.UI
{
    [RequireComponent(typeof(Text))]
    public class VersionLabelUGUI : VersionLabel, IUGUITextHolder
    {
        [field: SerializeField]
        public Text CachedText { get; set; }
        public override string Text { get => CachedText.text; set => CachedText.text = value; }
    }
}