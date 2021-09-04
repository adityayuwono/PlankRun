using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.Controllers.Inputs
{
    public class Touch : IHandleInput
    {
        private UnityEngine.Touch _touch;
        private Vector2 _initialPosition;

        public void ProcessForRotation(Action<float> OnRotate)
        {
            if (Input.touchCount == 1)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Began)
                {
                    _initialPosition = _touch.position;
                }
                else if (_touch.phase == TouchPhase.Moved || _touch.phase == TouchPhase.Stationary)
                {
                    var direction = _touch.position - _initialPosition;
                    var signedDirection = Mathf.Sign(direction.x);

                    OnRotate(signedDirection);
                }
            }
        }
    }
}
