using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;

// ========================
// Revision 2020.11.17
// ========================

namespace NightFramework
{
    public static class Utils
    {
        // ========================================================================================
        public static Object[] FindObjectsOfTypeAtSceneAll(Type type)
        {
            var result = Resources.FindObjectsOfTypeAll(type);
#if UNITY_EDITOR
            // exclude results, what is not in scene
            result = result.Where(x => !EditorUtility.IsPersistent(x)).ToArray();
#endif
            return result;
        }

        public static IEnumerable<T> FindObjectsOfTypeAtSceneAll<T>() where T : Object
        {
            var all = Resources.FindObjectsOfTypeAll(typeof(T));
            var result = all.Cast<T>();

#if UNITY_EDITOR
            // exclude results, what is not in scene
            result = result.Where(x => !EditorUtility.IsPersistent(x));
#endif
            return result;
        }

        public static void OpenURL(string url)
        {
#if UNITY_STANDALONE || UNITY_ANDROID || UNITY_IPHONE
            Application.OpenURL(url);
#endif
#if UNITY_WEBPLAYER || UNITY_WEBGL
            Application.ExternalEval(“window.open(‘”+url+”’);”);
#endif
        }
    }
}