using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace NightFramework.UI
{
    [DisallowMultipleComponent, RequireComponent(typeof(Selectable))]
    public class SelectThisOnEnable : MonoBehaviour
    {
        // ===========================================================================================
        private Selectable _selectable;


        // ===========================================================================================
        protected void Awake()
        {
            _selectable = GetComponent<Selectable>();
        }

        protected void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_selectable.gameObject);
        }
    }
}
