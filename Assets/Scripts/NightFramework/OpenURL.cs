using UnityEngine;

// ========================
// Revision 2020.11.17
// ========================

namespace NightFramework
{
    public class OpenURL : MonoBehaviour
    {
        // ===========================================================================================
        public string URL;


        // ===========================================================================================
        public void Open()
        {
            if (!string.IsNullOrWhiteSpace(URL))
                Utils.OpenURL(URL);
        }
    }
}