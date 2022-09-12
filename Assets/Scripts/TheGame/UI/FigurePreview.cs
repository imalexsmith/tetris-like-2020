using UnityEngine;
using UnityEngine.UI;
using UltEvents;

// ========================
// Revision 2020.10.28
// ========================

namespace TheGame.UI
{
    public class FigurePreview : MonoBehaviour
    {
        // ========================================================================================
        public FigureTypeKeys FigureType;
        public float BlockSize = 32f;
        public float VerticalBlocksCount = 2f;
        public Image[] BlockImages = new Image[16];
        public RectTransform CachedRectTransform;


        // ========================================================================================
        public void Generate(FigureTypeKeys fType)
        {
            var fTemplate = GameDataLibrary.Instance[fType];

            if (fType != FigureTypeKeys.None)
                CachedRectTransform.sizeDelta = new Vector2(fTemplate.VisualSize.x + 1f, VerticalBlocksCount + 1f) * BlockSize;

            var halfY = (Figure.LINE_BLOCKS_COUNT - fTemplate.VisualSize.y) / 2f;
            var lowerY = Mathf.Ceil(halfY);
            var upperY = Figure.LINE_BLOCKS_COUNT - Mathf.Floor(halfY);

            var halfX = (Figure.LINE_BLOCKS_COUNT - fTemplate.VisualSize.x) / 2f;
            var leftX = Mathf.Floor(halfX);
            var rightX = Figure.LINE_BLOCKS_COUNT - Mathf.Ceil(halfX);

            for (int x = 0; x < Figure.LINE_BLOCKS_COUNT; x++)
            {
                for (int y = 0; y < Figure.LINE_BLOCKS_COUNT; y++)
                {
                    var i = y * Figure.LINE_BLOCKS_COUNT + x;
                    if (x >= leftX && x < rightX && y >= lowerY && y < upperY)
                    {
                        BlockImages[i].gameObject.SetActive(true);
                        if (fTemplate.DefaultBlocks[i])
                        {
                            var bPrefab = GameDataLibrary.Instance[fTemplate.BlocksColor];
                            BlockImages[i].sprite = bPrefab.MainImage.sprite;
                            BlockImages[i].color = bPrefab.MainImage.color;
                        }
                        else
                        {
                            BlockImages[i].color = new Color(1f, 1f, 1f, 0f);
                        }
                    }
                    else
                    {
                        BlockImages[i].gameObject.SetActive(false);
                    }
                }
            }

            FigureType = fType;
        }

        protected void Awake()
        {
            if (!CachedRectTransform)
                CachedRectTransform = GetComponent<RectTransform>();
        }

#if UNITY_EDITOR
        [MyBox.ButtonMethod]
        private void Generate()
        {
            CachedRectTransform = GetComponent<RectTransform>();
            Generate(FigureType);
        }
#endif
    }
}