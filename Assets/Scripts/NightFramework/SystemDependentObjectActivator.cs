using UnityEngine;
using UnityEngine.SceneManagement;

// ========================
// Revision 2020.10.11
// ========================

namespace NightFramework
{
    [DisallowMultipleComponent]
    public sealed class SystemDependentObjectActivator : MonoBehaviour
    {
        // ===========================================================================================
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            var rootObjs = scene.GetRootGameObjects();
            foreach (var obj in rootObjs)
            {
                var list = obj.GetComponentsInChildren<SystemDependentObjectActivator>(true);
                foreach (var item in list)
                    CheckPlatform(item);
            }
        }

        private static void CheckPlatform(SystemDependentObjectActivator item)
        {
            if (item.IfDesktop == Action.DoNothing && item.IfAndorid == Action.DoNothing && item.IfIOs == Action.DoNothing)
                return;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.OSXPlayer:
                    if (item.IfDesktop == Action.Activate)
                        item.gameObject.SetActive(true);
                    else if (item.IfDesktop == Action.Deactivate)
                        item.gameObject.SetActive(false);
                    return;
                case RuntimePlatform.Android:
                    if (item.IfAndorid == Action.Activate)
                        item.gameObject.SetActive(true);
                    else if (item.IfAndorid == Action.Deactivate)
                        item.gameObject.SetActive(false);
                    return;
                case RuntimePlatform.IPhonePlayer:
                    if (item.IfIOs == Action.Activate)
                        item.gameObject.SetActive(true);
                    else if (item.IfIOs == Action.Deactivate)
                        item.gameObject.SetActive(false);
                    return;
            }
        }


        // ===========================================================================================
        public enum Action
        {
            DoNothing = 0,
            Activate = 1,
            Deactivate = 2
        }


        // ===========================================================================================
        [Header("Awake, OnEnable will still be called, even if Deactivate is selected!", order = 0)]
        [Space(order = 1)]
        public Action IfDesktop = Action.DoNothing;
        public Action IfAndorid = Action.DoNothing;
        public Action IfIOs = Action.DoNothing;
    }
}