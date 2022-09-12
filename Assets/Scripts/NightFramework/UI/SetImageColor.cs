using UnityEngine;
using UnityEngine.UI;


namespace NightFramework.UI
{
    [DisallowMultipleComponent, RequireComponent(typeof(Image))]
    public class SetImageColor : MonoBehaviour
    {
        // ===========================================================================================
        private Image _image;


        // ===========================================================================================
        public void SetColor(string color)
        {
            if (_image != null && !string.IsNullOrEmpty(color))
            {
                Color newC;

                if (ColorUtility.TryParseHtmlString(color, out newC))
                    _image.color = newC;
            }
        }

        protected void Awake()
        {
            _image = GetComponent<Image>();
        }
    }
}
