using UnityEngine;
using UltEvents;

// ========================
// Revision 2020.10.11
// ========================

namespace NightFramework
{
    public class OnSceneLoaded : MonoBehaviour
    {
        // ========================================================================================
        public UltEvent PreAwake = new UltEvent();
        public UltEvent PreStart = new UltEvent();
        public UltEvent PreFirstUpdate = new UltEvent();


        // ========================================================================================
        protected void Awake()
        {
            PreAwake.Invoke();
        }

        protected void Start()
        {
            PreStart.Invoke();
        }

        protected void Update()
        {
            PreFirstUpdate.Invoke();

            enabled = false;
        }
    }
}
