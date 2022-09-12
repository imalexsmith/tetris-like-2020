using UnityEngine;
using UnityEngine.Rendering.Universal;

// ========================
// Revision 2020.11.11
// ========================

namespace TheGame
{
    public class SetupPostProcessing : MonoBehaviour
    {
        // ========================================================================================  
        public UniversalAdditionalCameraData[] Cameras;


        // ========================================================================================  
        protected void Awake()
        {
            ApplicationSettings.Instance.OnPostProcessingChange += SetPostProcessing;
        }

        protected void Start()
        {
            SetPostProcessing();
        }

        protected void OnDestroy()
        {
            ApplicationSettings.Instance.OnPostProcessingChange -= SetPostProcessing;
        }

        private void SetPostProcessing()
        {
            foreach (var cameraData in Cameras)
                cameraData.renderPostProcessing = ApplicationSettings.Instance.PostProcessing;
        }
    }
}

