using UnityEngine;
using NightFramework;

// ========================
// Revision 2020.11.18
// ========================

namespace TheGame
{
    public static class OnGameLoading
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnGameLoaded()
        {
            if (ApplicationSettings.Instance.FirstLoad)
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Chinese:
                    case SystemLanguage.ChineseSimplified:
                    case SystemLanguage.ChineseTraditional:
                        ApplicationSettings.Instance.Language = Localization.Languages.ZH;
                        break;
                    case SystemLanguage.Russian:
                    case SystemLanguage.Ukrainian:
                    case SystemLanguage.Belarusian:
                        ApplicationSettings.Instance.Language = Localization.Languages.RU;
                        break;
                    case SystemLanguage.Spanish:
                        ApplicationSettings.Instance.Language = Localization.Languages.ES;
                        break;
                    case SystemLanguage.German:
                        ApplicationSettings.Instance.Language = Localization.Languages.DE;
                        break;
                    case SystemLanguage.Portuguese:
                        ApplicationSettings.Instance.Language = Localization.Languages.PT;
                        break;
                    case SystemLanguage.French:
                        ApplicationSettings.Instance.Language = Localization.Languages.FR;
                        break;
                    default:
                        ApplicationSettings.Instance.Language = Localization.Languages.EN;
                        break;
                }

                ApplicationSettings.Instance.SaveToFile();
                PersistentDataStorage.Instance.SaveToFile();
            }

            ApplicationSettings.Instance.LoadFromFile();
            PersistentDataStorage.Instance.LoadFromFile();
        }
    }
}
