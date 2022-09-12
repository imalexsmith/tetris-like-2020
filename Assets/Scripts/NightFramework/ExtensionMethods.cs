using System;
using UnityEngine;
using Object = UnityEngine.Object;

// ========================
// Revision 2020.11.17
// ========================

namespace NightFramework
{
    public static class ExtensionMethods
    {
        // ========================================================================================
        /// <summary>
        /// Destroy GameObject directly or GameObject to which this component is associated
        /// </summary>
        /// <returns>Always returns NULL</returns>
        public static T DestroyGameObject<T>(this T target) where T : Object
        {
            if (target == null)
                return null;

            // if target is GameObject itself - destroy it
            if (target is GameObject)
            {
                if (Application.isEditor && !Application.isPlaying)
                    Object.DestroyImmediate(target);
                else
                    Object.Destroy(target);

                return null;
            }

            // if target is attached Component - destroy its GameObject
            var t = target as Component;
            if (t != null)
            {
                if (Application.isEditor && !Application.isPlaying)
                    Object.DestroyImmediate(t.gameObject);
                else
                    Object.Destroy(t.gameObject);

                return null;
            }

            throw new InvalidCastException("The method \"DestroyGameObject\" is only available for classes that are GameObject, or Component, or have been inherited from any of them.");
        }

        public static bool Contains(this LayerMask layers, GameObject gameObject)
        {
            return 0 != (layers.value & 1 << gameObject.layer);
        }

        public static bool SetActiveSafe(this GameObject target, bool value)
        {
            if (value && target && !target.activeInHierarchy)
            {
                target.SetActive(true);
                return true;
            }

            if (!value && target && target.activeInHierarchy)
            {
                target.SetActive(false);
                return true;
            }

            return false;
        }
    }
}