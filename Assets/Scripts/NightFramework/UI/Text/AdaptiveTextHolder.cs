using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ========================
// Revision 2020.11.17
// ========================

namespace NightFramework.UI
{
    public interface ITMPTextHolder
    {
        TMP_Text CachedText { get; set; }
    }

    public interface IUGUITextHolder
    {
        Text CachedText { get; set; }
    }


    public abstract class AdaptiveTextHolder : MonoBehaviour
    {
        // ========================================================================================
        public abstract string Text { get; set; }


        // ========================================================================================
        protected virtual void Reset()
        {
            switch (this)
            {
                case ITMPTextHolder textHolder:
                    textHolder.CachedText = GetComponent<TMP_Text>();
                    return;
                case IUGUITextHolder textHolder:
                    textHolder.CachedText = GetComponent<Text>();
                    return;
                default:
                    throw new UnityException(Exceptions.AdaptiveTextHolderExpected);
            }
        }

        protected virtual void Awake()
        {
            switch (this)
            {
                case ITMPTextHolder textHolder:
                    if (!textHolder.CachedText)
                        textHolder.CachedText = GetComponent<TMP_Text>();
                    return;
                case IUGUITextHolder textHolder:
                    if (!textHolder.CachedText)
                        textHolder.CachedText = GetComponent<Text>();
                    return;
                default:
                    throw new UnityException(Exceptions.AdaptiveTextHolderExpected);
            }
        }
    }
}