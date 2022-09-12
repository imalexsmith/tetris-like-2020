using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// ========================
// Revision 2020.11.01
// ========================

namespace TheGame.UI
{
    public class OnHoldView : MonoBehaviour
    {
        // ========================================================================================
        public Image Background;
        public float BlinkTime = 0.3f;
        [Range(0f, 1f)]
        public float BlinkBias = 0.5f;
        public FigurePreview Preview;

        private Color _defaultBackgroundColor;
        private Sequence _colorBlinkTween;


        // ========================================================================================
        public void RefreshView()
        {
            var fTemplate = GameDataLibrary.Instance[GameField.Instance.FigureOnHold];
            var bPrefab = GameDataLibrary.Instance[fTemplate.BlocksColor];

            if (_colorBlinkTween.IsActive())
                _colorBlinkTween.Complete();

            _colorBlinkTween = DOTween.Sequence();
       
            _colorBlinkTween.Append(Background.DOColor(bPrefab.AverageMainImageColor, BlinkTime * (1f - BlinkBias)));
            _colorBlinkTween.Append(Background.DOColor(_defaultBackgroundColor, BlinkTime * BlinkBias));
            _colorBlinkTween.onComplete += () => { Background.color = _defaultBackgroundColor; };

            Preview.Generate(GameField.Instance.FigureOnHold);
        }

        protected void Start()
        {
            _defaultBackgroundColor = Background.color;

            GameField.Instance.OnPlaceOnHold += RefreshView;
            GameField.Instance.OnReload += RefreshView;
        }
    }
}
