using UnityEngine;

// ========================
// Revision 2020.10.11
// ========================

namespace NightFramework
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        // ========================================================================================
        protected static T _instance;
        public static T Instance => _instance;


        // ========================================================================================
        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void OnEnable()
        {
            Initialize();
        }

        protected virtual void OnDisable()
        {
            if (_instance == this)
                _instance = null;
        }

        private void Initialize()
        {
            if (_instance == null)
                _instance = (T)this;
            else if (_instance != this)
                throw new UnityException($"There cannot be more than one {typeof(T).Name} script in scene. The instances {_instance.name} and {name} were found.");
        }
    }
}