using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UltEvents;

// ========================
// Revision 2020.03.02
// ========================

namespace NightFramework.UI
{
    [DisallowMultipleComponent]
    public abstract class TextLocalizator<T> : MonoBehaviour where T : UIBehaviour
    {
        // ========================================================================================
        public UltEvent OnTranslated = new UltEvent();
        
        [Space]
        public Localization LocalizationPack;
        public bool AutoTranslateOnEnable = true;
        public bool AutoTranslateOnLanguageChange = true;

        [SerializeField]
        private T _cachedText;
        public T CachedText
        {
            get => _cachedText;
            protected set => _cachedText = value;
        }

        [SerializeField]
        private string _key;
        public string Key
        {
            get => _key;
            set => _key = value;
        }
        

        // ========================================================================================
        public void Translate()
        {
            Translate(ApplicationSettingsBase.Instance.Language);
        }

        public void Translate(Localization.Languages language)
        {
            if (LocalizationPack == null || string.IsNullOrWhiteSpace(Key))
                return;

            var val = LocalizationPack.GetValue(Key, language);
            if (string.IsNullOrWhiteSpace(val))
                val = LocalizationPack.GetValue(Key, Localization.Languages.EN);

            SetText(val);
            OnTranslated.Invoke();
        }

        public void AssignRandomKey()
        {
            if (LocalizationPack != null)
            {
                var keys = LocalizationPack.GetKeys().ToList();
                var i = Random.Range(0, keys.Count);

                Key = keys[i];
            }
        }

        protected abstract void SetText(string text);

        protected void Awake()
        {
            if (!_cachedText)
                _cachedText = GetComponent<T>();
        }

        protected void OnEnable()
        {
            if (AutoTranslateOnEnable)
                Translate();
        }

        protected void Start()
        {
            ApplicationSettingsBase.Instance.OnLanguageChange += OnLanguageChange;
        }

        protected void OnDestroy()
        {
            ApplicationSettingsBase.Instance.OnLanguageChange -= OnLanguageChange;
        }

        private void OnLanguageChange(Localization.Languages oldVal, Localization.Languages newVal)
        {
            if (AutoTranslateOnLanguageChange)
                Translate(newVal);
        }
    }
}
