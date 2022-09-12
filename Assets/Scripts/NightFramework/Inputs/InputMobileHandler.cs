using System;
using UnityEngine;
using UltEvents;

// ========================
// Revision 2019.11.06
// ========================

namespace NightFramework.Inputs
{
    public class InputMobileHandler : InputHandlerBase
    {
        // ========================================================================================
        /// <summary>
        ///  Minimum distance (in percent of screen height) for a swipe to be registered
        /// </summary>
        public static float MinDragDistance = 0.06f;

        private bool _touched;
        private Vector3 _firstTouchPos;
        private Vector3 _lastTouchPos;


        // ========================================================================================
        protected override void UpdateInput()
        {
            throw new NotImplementedException();

            // ...or read mobile input if single touch registered only
            /*else if (Input.touchCount == 1)
            {
                if (SelectHeroArrow != null)
                    SelectHeroArrow.SetActive(false);

                var touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            _touched = false;
                            break;
                        }

                        _touched = true;
                        _firstTouchPos = touch.position;
                        _lastTouchPos = touch.position;
                        break;
                    case TouchPhase.Moved:
                        if (_touched)
                            _lastTouchPos = touch.position;
                        break;
                    case TouchPhase.Ended:
                        if (_touched)
                        {
                            _touched = false;
                            _lastTouchPos = touch.position;

                            var xDistance = Mathf.Abs(_lastTouchPos.x - _firstTouchPos.x);
                            var yDistance = Mathf.Abs(_lastTouchPos.y - _firstTouchPos.y);
                            var dragDistance = MinDragDistance * Screen.height;

                            if (xDistance > dragDistance || yDistance > dragDistance)
                            {
                                if (xDistance > yDistance)
                                {
                                    result = _lastTouchPos.x > _firstTouchPos.x
                                        ? Hero.ActionType.MoveRight
                                        : Hero.ActionType.MoveLeft;
                                }
                                else
                                {
                                    result = _lastTouchPos.y > _firstTouchPos.y
                                        ? Hero.ActionType.MoveUp
                                        : Hero.ActionType.MoveDown;
                                }
                            }
                            else
                                //It's a tap as the drag distance is less than min distance
                                result = Hero.ActionType.ResetStatuses;
                        }
                        break;
                }
            }*/
        }
    }
}