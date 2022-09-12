using UnityEngine;
using NightFramework;
using UnityEngine.Serialization;

// ========================
// Revision 2020.10.31
// ========================

namespace TheGame
{
    [RequireComponent(typeof(Animator), typeof(TrailRenderer), typeof(ParticleSystem))]
    public class FigureBlock : PoolableObject
    {
        // ========================================================================================
        public FigureBlockColorKeys BlockColor;
        public float SpinFactor;
        [Range(0f, 1f)]
        public float SpinOffset = 1f;
        public SpriteRenderer MainImage;
        public Animator CachedAnimator;
        public ParticleSystem CachedParticles;

        [Range(1f, 2f), FormerlySerializedAs("AverageImageColorMultiplier")]
        public float AverageMainImageColorMultiplier = 1.45f;
        [field: SerializeField, ReadOnly]
        public Color AverageMainImageColor { get; private set; }

        private MaterialPropertyBlock _materialPropertyBlock;

        private readonly int _finalizeTriggerHash = Animator.StringToHash("Finalize");
        private readonly int _removeTriggerHash = Animator.StringToHash("Remove");

        private readonly int _spinFactorId = Shader.PropertyToID("_SpinFactor");
        private readonly int _spinOffsetId = Shader.PropertyToID("_SpinOffset");


        // ========================================================================================
        public void FinalizeAnim()
        {
            CachedAnimator.SetTrigger(_finalizeTriggerHash);
        }

        public void RemoveAnim()
        {
            CachedAnimator.SetTrigger(_removeTriggerHash);
            CachedParticles.Play();
        }

        protected void Awake()
        {
            if (!CachedAnimator)
                CachedAnimator = GetComponent<Animator>();

            if (!CachedParticles)
                CachedParticles = GetComponent<ParticleSystem>();

            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        protected void LateUpdate()
        {
            MainImage.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(_spinFactorId, SpinFactor);
            _materialPropertyBlock.SetFloat(_spinOffsetId, SpinOffset);
            MainImage.SetPropertyBlock(_materialPropertyBlock);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            AverageMainImageColor = AverageColorFromTexture(MainImage.sprite.texture);
            AverageMainImageColor *= AverageMainImageColorMultiplier;

            _materialPropertyBlock = new MaterialPropertyBlock();

            MainImage.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat(_spinFactorId, SpinFactor);
            _materialPropertyBlock.SetFloat(_spinOffsetId, SpinOffset);
            MainImage.SetPropertyBlock(_materialPropertyBlock);
        }

        private Color32 AverageColorFromTexture(Texture2D tex)
        {
            Color32[] texColors = tex.GetPixels32();

            int total = texColors.Length;

            float r = 0f;
            float g = 0f;
            float b = 0f;

            for (int i = 0; i < total; i++)
            {
                r += texColors[i].r;
                g += texColors[i].g;
                b += texColors[i].b;
            }

            return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), byte.MaxValue);
        }
#endif
    }
}