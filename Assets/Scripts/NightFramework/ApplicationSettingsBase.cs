using System;
using System.IO;
using System.Text;
using UnityEngine;

// ========================
// Revision 2020.10.22
// ========================

namespace NightFramework
{
    public class ApplicationSettingsBase : ScriptableObject, ISaveLoadFile
    {
        public const string FileName = "settings.txt";
        public const float AudioMin = 0.0001f;
        public const float AudioMax = 1f;


        // ========================================================================================
        private static ApplicationSettingsBase _instance;
        public static ApplicationSettingsBase Instance
        {
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
            get
            {
                if (_instance == null)
                {
                    var assets = Resources.FindObjectsOfTypeAll<ApplicationSettingsBase>();
                    if (assets.Length == 1)
                        _instance = assets[0];
                    else
                        throw new UnityException(Exceptions.ApplicationSettingsMustBeUnique);
                }

                return _instance;
            }
        }


        // ========================================================================================  
        public string FileFullPath => Path.Combine(Application.persistentDataPath, FileName);

        public bool FirstLoad => !File.Exists(FileFullPath);

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            private set
            {
                if (value != _isLoaded)
                    _isLoaded = value;
            }
        }

        public event Action OnLoaded;

        [Header("Independent properties")]
        [SerializeField, ReadOnly]
        private string _appID;
        public string AppID
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _appID;
            }
            private set
            {
                if (value != _appID)
                    _appID = value;
            }
        }

        [SerializeField]
        private InputType _currentInputType = InputType.Undefined;
        public InputType CurrentInputType
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _currentInputType;
            }
            set
            {
                if (value != _currentInputType)
                {
                    var oldValue = _currentInputType;
                    _currentInputType = value;

                    OnCurrentInputTypeChange?.Invoke(oldValue, value);
                }
            }
        }

        public event Action<InputType, InputType> OnCurrentInputTypeChange;

        [SerializeField, Min(0f)]
        private float _mouseSensitivity = 1f;
        public float MouseSensitivity
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _mouseSensitivity;
            }
            set
            {
                if (value != _mouseSensitivity)
                {
                    var oldValue = _mouseSensitivity;
                    _mouseSensitivity = value;

                    OnMouseSensitivityChange?.Invoke(oldValue, value);
                }
            }
        }

        public event Action<float, float> OnMouseSensitivityChange;

        [SerializeField, Min(0f)]
        private float _controllerSensitivity = 1f;
        public float ControllerSensitivity
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _controllerSensitivity;
            }
            set
            {
                if (value != _controllerSensitivity)
                {
                    var oldValue = _controllerSensitivity;
                    _controllerSensitivity = value;

                    OnControllerSensitivityChange?.Invoke(oldValue, value);
                }
            }
        }

        public event Action<float, float> OnControllerSensitivityChange;

        [SerializeField, Range(AudioMin, AudioMax)]
        private float _masterVolume = 0.55f;
        public float MasterVolume
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _masterVolume;
            }
            set
            {
                value = Mathf.Clamp(value, AudioMin, AudioMax);

                if (value != _masterVolume)
                {
                    var oldValue = _masterVolume;
                    _masterVolume = value;

                    OnMasterVolumeChange?.Invoke(oldValue, value);
                }
            }
        }

        public event Action<float, float> OnMasterVolumeChange;

        [SerializeField, Range(AudioMin, AudioMax)]
        private float _musicVolume = 0.55f;
        public float MusicVolume
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _musicVolume;
            }
            set
            {
                value = Mathf.Clamp(value, AudioMin, AudioMax);

                if (value != _musicVolume)
                {
                    var oldValue = _musicVolume;
                    _musicVolume = value;

                    OnMusicVolumeChange?.Invoke(oldValue, value);
                }
            }
        }

        public event Action<float, float> OnMusicVolumeChange;

        [SerializeField, Range(AudioMin, AudioMax)]
        private float _sfxVolume = 0.55f;
        public float SfxVolume
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _sfxVolume;
            }
            set
            {
                value = Mathf.Clamp(value, AudioMin, AudioMax);

                if (value != _sfxVolume)
                {
                    var oldValue = _sfxVolume;
                    _sfxVolume = value;

                    OnSfxVolumeChange?.Invoke(oldValue, value);
                }
            }
        }

        public event Action<float, float> OnSfxVolumeChange;

        [SerializeField, Range(AudioMin, AudioMax)]
        private float _voiceVolume = 0.55f;
        public float VoiceVolume
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _voiceVolume;
            }
            set
            {
                value = Mathf.Clamp(value, AudioMin, AudioMax);

                if (value != _voiceVolume)
                {
                    var oldValue = _voiceVolume;
                    _voiceVolume = value;

                    OnVoiceVolumeChange?.Invoke(oldValue, value);
                }
            }
        }

        public event Action<float, float> OnVoiceVolumeChange;

        [SerializeField]
        private bool _showFps = true;
        public bool ShowFps
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _showFps;
            }
            set
            {
                if (value != _showFps)
                {
                    _showFps = value;

                    OnShowFpsChange?.Invoke();
                }
            }
        }

        public event Action OnShowFpsChange;

        [SerializeField]
        private bool _animMenus = true;
        public bool AnimMenus
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _animMenus;
            }
            set
            {
                if (value != _animMenus)
                {
                    _animMenus = value;

                    OnAnimMenusChange?.Invoke();
                }
            }
        }

        public event Action OnAnimMenusChange;

        [SerializeField]
        private bool _postProcessing = true;
        public bool PostProcessing
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _postProcessing;
            }
            set
            {
                if (value != _postProcessing)
                {
                    _postProcessing = value;

                    OnPostProcessingChange?.Invoke();
                }
            }
        }

        public event Action OnPostProcessingChange;

        [SerializeField]
        private Localization.Languages _language = Localization.Languages.EN;
        public Localization.Languages Language
        {
            get
            {
                if (!IsLoaded)
                    LoadFromFile();

                return _language;
            }
            set
            {
                if (value != _language)
                {
                    var oldValue = _language;
                    _language = value;

                    OnLanguageChange?.Invoke(oldValue, value);
                }
            }
        }

        public event Action<Localization.Languages, Localization.Languages> OnLanguageChange;



        // ========================================================================================
        public void SaveToFile(bool compressed = true)
        {
            if (string.IsNullOrWhiteSpace(AppID))
                AppID = $"{SystemInfo.deviceUniqueIdentifier}:{Application.platform}";

            var dat = JsonUtility.ToJson(this, !compressed);
            File.WriteAllText(FileFullPath, dat, Encoding.UTF8);
        }

        public bool LoadFromFile()
        {
            if (File.Exists(FileFullPath))
            {
                var text = File.ReadAllText(FileFullPath, Encoding.UTF8);
                JsonUtility.FromJsonOverwrite(text, this);

                IsLoaded = true;
                OnLoaded?.Invoke();

                return true;
            }

            return false;
        }

        public void SelectNextLanguage()
        {
            if (Enum.IsDefined(typeof(Localization.Languages), _language + 1))
                Language++;
            else
                Language = Localization.Languages.EN;
        }
    }
}