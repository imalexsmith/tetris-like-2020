using System.Collections.Generic;
using UnityEngine;

// ========================
// Revision 2020.11.12
// ========================

namespace NightFramework.Inputs
{
    public class InputHandlersGroup : MonoBehaviour
    {
        // ========================================================================================
        public static HashSet<InputHandlersGroup> AvailableHandlersGroups
        {
            get;
        } = new HashSet<InputHandlersGroup>();

        public static InputHandlersGroup ActiveGroup
        {
            get;
            protected set;
        }

        private static readonly Stack<InputHandlersGroup> _activationStack = new Stack<InputHandlersGroup>();
        
        public static void ClearActivationStack()
        {
            _activationStack.Clear();
            ActiveGroup = null;
        }


        // ========================================================================================
        public bool ReactivatePrevious;

        private readonly HashSet<InputHandlerBase> _handlers = new HashSet<InputHandlerBase>();
        

        // ========================================================================================
        public void RegisterHandler(InputHandlerBase handler)
        {
            if (handler != null)
            {
                if (_handlers.Add(handler))
                    handler.enabled = false;
            }
        }

        public void UnregisterHandler(InputHandlerBase handler)
        {
            _handlers.Remove(handler);
        }

        public void ActivateGroup()
        {
            if (ActiveGroup == this)
                return;

            foreach (var group in AvailableHandlersGroups)
            {
                var activate = group == this ? true : false;
                group.UpdateHandlers(activate);
            }

            _activationStack.Push(this);
            ActiveGroup = this;
        }

        public void DeactivateGroup()
        {
            if (ActiveGroup != this)
                return;

            UpdateHandlers(false);
            _activationStack.Pop();
            ActiveGroup = null;

            if (ReactivatePrevious && _activationStack.Count > 0)
            {
                var prev = _activationStack.Peek();
                if (AvailableHandlersGroups.Contains(prev))
                {
                    prev.UpdateHandlers(true);
                    ActiveGroup = prev;
                }
            }
        }

        protected void OnEnable()
        {
            AvailableHandlersGroups.Add(this);
        }

        protected void OnDisable()
        {
            AvailableHandlersGroups.Remove(this);
            UpdateHandlers(false);
        }

        private void UpdateHandlers(bool activate)
        {
            foreach (var handler in _handlers)
                handler.enabled = activate;
        }
    }
}