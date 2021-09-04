﻿using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.Controllers.Inputs
{
    public class Mouse : IHandleInput
    {
        private bool _isDown;
        private Vector3 _initialPosition;

        public void ProcessForRotation(Action<float> OnRotate)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_isDown)
                {
                    _isDown = true;
                    _initialPosition = Input.mousePosition;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_isDown)
                {
                    var direction = Input.mousePosition - _initialPosition;
                    var signedDirection = Mathf.Sign(direction.x);

                    OnRotate(signedDirection);
                }

                _isDown = false;
            }
        }
    }
}
