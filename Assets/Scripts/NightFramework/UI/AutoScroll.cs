using UnityEngine;
using UnityEngine.UI;

// ========================
// Revision 2020.09.11
// ========================

namespace NightFramework.UI
{
    public class AutoScroll : MonoBehaviour
    {
        // ========================================================================================
        public float Delay = 0.5f;
        public float Amount = 1f;
        public Scrollbar TargetScrollbar;

        private float _allowedTime;


        // ========================================================================================
        protected void OnEnable()
        {
            _allowedTime = Time.time + Delay;
        }

        protected void Update()
        {
            if (Time.time >= _allowedTime)
                TargetScrollbar.value += Amount * Time.deltaTime;
        }
    }
}