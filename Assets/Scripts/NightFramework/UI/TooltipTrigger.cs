using UnityEngine;
using UnityEngine.EventSystems;
using UltEvents;
using MyBox;

// ========================
// Revision 2020.02.29
// ========================

namespace NightFramework.UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // ========================================================================================
        public UltEvent PointerEnter = new UltEvent();
        public UltEvent PointerExit = new UltEvent();

        [Space]
        public Tooltip TooltipRoot;
        public Vector3 Offset;
        public float Delay;

        public bool UseCustomImage;
        [ConditionalField(nameof(UseCustomImage)), SerializeField]
        private Sprite _customImage;
        public Sprite CustomImage 
        {
            get => _customImage; 
            set => _customImage = value; 
        }

        public bool UseCustomText;
        [ConditionalField(nameof(UseCustomText)), SerializeField]
        private string _customText;
        public string CustomText 
        {
            get => _customText; 
            set => _customText = value;
        }

        public bool UseCustomLocalization;
        [ConditionalField(nameof(UseCustomLocalization)), SerializeField]
        private string _localizationKey;
        public string LocalizationKey 
        {
            get => _localizationKey; 
            set => _localizationKey = value; 
        }


        // ========================================================================================
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter.Invoke();

            if (TooltipRoot != null)
            {
                if (UseCustomImage)
                { 
                    if (TooltipRoot.TooltipImage != null)
                        TooltipRoot.TooltipImage.sprite = CustomImage;
                }

                if (UseCustomText)
                {
                    if (TooltipRoot.TooltipText != null)
                        TooltipRoot.TooltipText.text = CustomText;
                }
                else
                {
                    if (UseCustomLocalization)
                    {
                        if (TooltipRoot.TooltipLocText != null)
                        {
                            TooltipRoot.TooltipLocText.Key = LocalizationKey;
                            TooltipRoot.TooltipLocText.Translate();
                        }
                    }
                }

                TooltipRoot.Show(Offset, Delay);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit.Invoke();

            if (TooltipRoot != null)
                TooltipRoot.Hide();
        }
    }
}