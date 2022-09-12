using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// ========================
// Revision 2020.10.30
// ========================

namespace TheGame.UI
{ 
    public class NextFiguresView : MonoBehaviour
    {
        // ========================================================================================
        public int PreviewsCount = 5;
        public Vector3 FirstPreviewScale = new Vector3(1.15f, 1.15f, 1.15f);
        public float CollapseTime = 0.2f;
        public FigurePreview PreviewPrefab;

        private List<FigurePreview> _previews = new List<FigurePreview>();
        private Tweener _collapseTween;


        // ========================================================================================
        public void RefreshView()
        {
            if (_collapseTween.IsActive())
                _collapseTween.Complete();

            _previews[0].Generate(FigureTypeKeys.None);
            _previews[0].transform.localScale = Vector3.one;
            
            _previews[1].CachedRectTransform.DOScale(FirstPreviewScale, CollapseTime);
            _collapseTween = _previews[0].CachedRectTransform.DOSizeDelta(new Vector2(_previews[0].CachedRectTransform.sizeDelta.x, 0f), CollapseTime);
            _collapseTween.onComplete += () => 
            {
                var t = _previews[0];
                for (int i = 0; i < _previews.Count - 1; i++)
                    _previews[i] = _previews[i + 1];

                _previews[_previews.Count - 1] = t;
                
                t.transform.SetAsLastSibling();
            };

            var j = 1;
            foreach (var figure in GameField.Instance.NextFiguresSet)
            {
                _previews[j].Generate(figure);
                j++;

                if (j > PreviewsCount)
                    break;
            }
        }

        protected void Start()
        {
            for (int i = 0; i < PreviewsCount + 1; i++)
                _previews.Add(Instantiate(PreviewPrefab, transform));

            GameField.Instance.OnNewFigure += RefreshView;
            GameField.Instance.OnReload += RefreshView;
        }
    }
}
