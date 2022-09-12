using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UltEvents;
using MyBox;

// ========================
// Revision 2020.10.17
// ========================

namespace NightFramework.UI
{
    public interface IMenuMethods
    {
        void Switch();
        void Switch(bool animate);
        void Open();
        void Open(bool animate);
        void Close();
        void Close(bool animate);
    }

    [DisallowMultipleComponent, RequireComponent(typeof(Animator))]
    public class MenuBase2 : MonoBehaviour, IMenuMethods
    {
        public enum MenuStates
        {
            Closed = 0,
            Opened = 1
        }

        public enum CloseAction
        {
            Disable = 0,
            Destroy = 1,
            DoNothing = 2
        }


        // ========================================================================================
        private static readonly List<MenuBase2> _openedMenus = new List<MenuBase2>();
        public static ReadOnlyCollection<MenuBase2> OpenedMenus { get; private set; }


        // ========================================================================================
        public static void CloseAll()
        {
            _openedMenus.ForEach(x => x.Close());
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            OpenedMenus = _openedMenus.AsReadOnly();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            var rootObjs = scene.GetRootGameObjects();
            foreach (var obj in rootObjs)
            {
                var list = obj.GetComponentsInChildren<MenuBase2>(true);
                foreach (var item in list)
                {
                    switch (item.InitialState)
                    {
                        case MenuStates.Closed:
                            item.ForceClose(false);
                            break;
                        case MenuStates.Opened:
                            item.ForceOpen(false);
                            break;
                    }
                }
            }
        }


        // ========================================================================================
        [Space]
        public UltEvent OnBeforeOpen = new UltEvent();
        public UltEvent OnOpenStarted = new UltEvent();
        public UltEvent OnOpenCompleted = new UltEvent();
        public UltEvent OnBeforeClose = new UltEvent();
        public UltEvent OnCloseStarted = new UltEvent();
        public UltEvent OnCloseCompleted = new UltEvent();

        [Space]
        public MenuStates InitialState;
        public MenuStates CurrentState { get; protected set; }
        public CloseAction WhenClosed;

        [Space]
        public bool NeedOpenAnimation = true;
        public float OpenStartDelay = 0f;
        [ConditionalField(nameof(NeedOpenAnimation))]
        public string OpenAnimTrigger = "Open";
        [ConditionalField(nameof(NeedOpenAnimation))]
        public string OpenedAnimState = "Opened";

        [Space]
        public bool NeedCloseAnimation = true;
        public float CloseStartDelay = 0f;
        [ConditionalField(nameof(NeedCloseAnimation))]
        public string CloseAnimTrigger = "Close";
        [ConditionalField(nameof(NeedCloseAnimation))]
        public string ClosedAnimState = "Closed";

        [SerializeField, HideInInspectorIfNotDebug]
        private Animator _animator;


        // ========================================================================================
        public void Switch()
        {
            switch (CurrentState)
            {
                case MenuStates.Closed:
                    Open();
                    break;
                case MenuStates.Opened:
                    Close();
                    break;
            }
        }

        public void Switch(bool animate)
        {
            switch (CurrentState)
            {
                case MenuStates.Closed:
                    Open(animate);
                    break;
                case MenuStates.Opened:
                    Close(animate);
                    break;
            }
        }

        public void Open()
        {
            Open(NeedOpenAnimation);
        }

        public void Open(bool animate)
        {
            if (CurrentState == MenuStates.Opened)
                return;

            BeforeOpen(true);

            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(Opening(animate, OpenStartDelay, true));
        }

        public void Close()
        {
            Close(NeedCloseAnimation);
        }

        public void Close(bool animate)
        {
            if (CurrentState == MenuStates.Closed)
                return;

            BeforeClose(true);

            StopAllCoroutines();
            StartCoroutine(Closing(animate, CloseStartDelay, true));
        }

        public void DeselectAllSelectables()
        {
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }

        protected void ForceOpen(bool withEvents)
        {
            BeforeOpen(withEvents);
            gameObject.SetActive(true);

            _animator.ResetTrigger(CloseAnimTrigger);
            _animator.ResetTrigger(OpenAnimTrigger);
            _animator.Play(OpenedAnimState, -1, 1f);
            OpenComplete(withEvents);
        }

        protected void ForceClose(bool withEvents)
        {
            BeforeClose(withEvents);

            if (_animator.isActiveAndEnabled)
            {
                _animator.ResetTrigger(CloseAnimTrigger);
                _animator.ResetTrigger(OpenAnimTrigger);
                _animator.Play(ClosedAnimState, -1, 1f);
            }
            CloseComplete(withEvents);
        }

        protected IEnumerator Opening(bool animate, float delay, bool withEvents)
        {
            if (delay > 0 && ApplicationSettingsBase.Instance.AnimMenus)
                yield return new WaitForSeconds(delay);

            _animator.ResetTrigger(CloseAnimTrigger);
            _animator.ResetTrigger(OpenAnimTrigger);

            if (withEvents)
                OnOpenStarted.Invoke();

            if (animate && ApplicationSettingsBase.Instance.AnimMenus)
            {
                _animator.SetTrigger(OpenAnimTrigger);

                while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(OpenedAnimState))
                    yield return null;

                OpenComplete(withEvents);
            }
            else
            {
                _animator.Play(OpenedAnimState, -1, 1f);
                OpenComplete(withEvents);
            }
        }

        protected IEnumerator Closing(bool animate, float delay, bool withEvents)
        {
            if (delay > 0 && ApplicationSettingsBase.Instance.AnimMenus)
                yield return new WaitForSeconds(delay);

            _animator.ResetTrigger(CloseAnimTrigger);
            _animator.ResetTrigger(OpenAnimTrigger);

            if (withEvents)
                OnCloseStarted.Invoke();

            if (animate && ApplicationSettingsBase.Instance.AnimMenus)
            {
                _animator.SetTrigger(CloseAnimTrigger);

                while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(ClosedAnimState))
                    yield return null;

                CloseComplete(withEvents);
            }
            else
            {
                _animator.Play(ClosedAnimState, -1, 1f);
                CloseComplete(withEvents);
            }
        }

        protected void Reset()
        {
            SetupAnimator();
        }

        protected void Awake()
        {
            SetupAnimator();
        }

        protected void OnDestroy()
        {
            if (_openedMenus.Contains(this))
                _openedMenus.Remove(this);
        }

        private void BeforeOpen(bool withEvents)
        {
            if (withEvents)
                OnBeforeOpen.Invoke();

            CurrentState = MenuStates.Opened;

            if (!_openedMenus.Contains(this))
                _openedMenus.Add(this);
        }

        private void BeforeClose(bool withEvents)
        {
            if (withEvents)
                OnBeforeClose.Invoke();

            CurrentState = MenuStates.Closed;

            if (_openedMenus.Contains(this))
                _openedMenus.Remove(this);
        }

        private void OpenComplete(bool withEvents)
        {
            if (withEvents)
                OnOpenCompleted.Invoke();
        }

        private void CloseComplete(bool withEvents)
        {
            if (withEvents)
                OnCloseCompleted.Invoke();

            switch (WhenClosed)
            {
                case CloseAction.Disable:
                    gameObject.SetActive(false);
                    break;
                case CloseAction.Destroy:
                    gameObject.DestroyGameObject();
                    break;
                case CloseAction.DoNothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupAnimator()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                _animator.keepAnimatorControllerStateOnDisable = true;
            }
        }
    }
}