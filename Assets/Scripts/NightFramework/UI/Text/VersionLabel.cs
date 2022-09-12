using UnityEngine;

// ========================
// Revision 2020.11.17
// ========================

namespace NightFramework.UI
{
    public abstract class VersionLabel : AdaptiveTextHolder
    {
        // ===========================================================================================
        protected override void Awake()
        {
            base.Awake();

            Text = $"ver. {Application.version}";
        }
    }
}