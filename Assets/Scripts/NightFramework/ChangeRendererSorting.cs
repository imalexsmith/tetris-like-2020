using UnityEngine;

// ========================
// Revision 2020.11.06
// ========================

namespace NightFramework
{
    [DisallowMultipleComponent, RequireComponent(typeof(Renderer)), ExecuteAlways]
    public class ChangeRendererSorting : MonoBehaviour
    {
        // ===========================================================================================
        public Renderer TargetRenderer;
        public int Offset;
        public AutoUpdateOption AutoUpdate = AutoUpdateOption.Both;
        public Renderer CachedRenderer;

        private int _lastTargetSortingLayer;
        private int _lastTargetSortingOrder;
        private int _lastOffset;


        // ===========================================================================================
        public void UpdateSorting()
        {
            if (TargetRenderer != null &&
                (TargetRenderer.sortingLayerID != _lastTargetSortingLayer || TargetRenderer.sortingOrder != _lastTargetSortingOrder || Offset != _lastOffset))
            {
                CachedRenderer.sortingLayerID = TargetRenderer.sortingLayerID;
                CachedRenderer.sortingOrder = TargetRenderer.sortingOrder + Offset;

                _lastTargetSortingLayer = TargetRenderer.sortingLayerID;
                _lastTargetSortingOrder = TargetRenderer.sortingOrder;
            }

            _lastOffset = Offset;
        }

        protected void Reset()
        {
            CachedRenderer = GetComponent<Renderer>();
        }

        protected void Awake()
        {
            if (!CachedRenderer)
                CachedRenderer = GetComponent<Renderer>();
        }

        protected void Start()
        {
            if (AutoUpdate == AutoUpdateOption.Both || AutoUpdate == AutoUpdateOption.InBuild)
                UpdateSorting();
        }

        protected void LateUpdate()
        {
#if UNITY_EDITOR
            if (AutoUpdate == AutoUpdateOption.Both || AutoUpdate == AutoUpdateOption.InEditor)
            {
                UpdateSorting();
                return;
            }
#endif

            if (AutoUpdate == AutoUpdateOption.Both || AutoUpdate == AutoUpdateOption.InBuild)
                UpdateSorting();
        }
    }
}