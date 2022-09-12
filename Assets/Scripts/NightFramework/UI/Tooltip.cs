using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UltEvents;
using TMPro;

// ========================
// Revision 2020.03.02
// ========================

namespace NightFramework.UI
{
    public class Tooltip : MonoBehaviour
    {
        // ========================================================================================
        public UltEvent OnShow = new UltEvent();
        public UltEvent OnHide = new UltEvent();

        [Space]
        public bool MouseFollow = true;
        public GameObject TooltipBackground;
        public Image TooltipImage;
        public TMP_Text TooltipText;
        public TextLocalizatorTMP TooltipLocText;

        private Vector3 _defaultOffset;
        private Vector3 _offset;
        private float _activationDelay;
        private bool _isInShow;
        private Canvas _parentCanvas;
        private RectTransform _rectTransform;


        // ========================================================================================
        public void Show(Vector3 offset, float delay)
        {
            _offset = offset;
            _activationDelay = delay;

            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(ShowDelayed(_activationDelay));
        }

        public void Show()
        {
            Show(_defaultOffset, 0f);
        }

        public void Hide()
        {
            HideNow();

            gameObject.SetActive(false);
        }

        protected void Start()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
            _rectTransform = transform as RectTransform;
            _defaultOffset = transform.position;

            Hide();
        }

        protected void LateUpdate()
        {
            if (_isInShow)
            {
                var newPos = _offset;

                if (MouseFollow)
                    newPos += Input.mousePosition;

                if (_parentCanvas.renderMode == RenderMode.ScreenSpaceCamera && _parentCanvas.worldCamera != null)
                {
                    newPos = _parentCanvas.worldCamera.ScreenToWorldPoint(newPos);
                    newPos.z = 0f;
                }

                transform.position = newPos;
            }
        }

        private IEnumerator ShowDelayed(float delay)
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            ShowNow();
        }

        private void ShowNow()
        {
            if (TooltipBackground != null)
                TooltipBackground.SetActive(true);

            _isInShow = true;

            OnShow.Invoke();
        }

        private void HideNow()
        {
            if (TooltipBackground != null)
                TooltipBackground.SetActive(false);

            _isInShow = false;

            OnHide.Invoke();
        }
    }
}