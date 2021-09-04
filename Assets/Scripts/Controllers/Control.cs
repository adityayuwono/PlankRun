﻿using Assets.Scripts.Constants;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class Control : IControlCharacter
    {
        private Transform _transform;
        private CharacterController _controller;

        private Transform _waterRayOrigin;
        private float _jumpHeight;
        private SpeedModel _speed;
        private Quaternion _turnTarget;
        private RaycastHit hitInfo;
        private Vector3 _playerVelocity;
        private Character _character;

        private Animator _animator;
        private bool _isJumping;

        public Control(CharacterModel model, Character character)
        {
            _character = character;
            _animator = new Animator(model.GameObject, model.Speed.Movement);

            var gameObject = model.GameObject;

            _transform = gameObject.transform;
            _controller = gameObject.GetComponent<CharacterController>();

            _jumpHeight = model.JumpHeight;
            _speed = model.Speed;
            _waterRayOrigin = model.WaterRayOrigin;

            _turnTarget = _transform.rotation;

            hitInfo = new RaycastHit();
        }

        public void Move()
        {
            if (_controller.isGrounded)
            {
                if (_isJumping)
                {
                    _isJumping = false;
                    _animator.Land();
                }
                else
                {
                    _animator.StartRunning();
                }
            }

            var ray = new Ray(_waterRayOrigin.position, Vector3.down);
            
            if (_controller.isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.name == "Water" && _controller.isGrounded)
                {
                    _character.HandleNearWater(hitInfo);
                }
            }

            _playerVelocity.y += WorldValues.Gravity * Time.deltaTime;

            Vector3 move = _transform.forward.normalized * _speed.Movement;
            move.y = _playerVelocity.y;

            _controller.Move(move * Time.deltaTime);

            if (_controller.isGrounded)
            {
                var turnTarget = Quaternion.SlerpUnclamped(_transform.rotation, _turnTarget, Time.deltaTime);
                _transform.rotation = turnTarget;
            }
        }

        public void Rotate(float signedDirection)
        {
            _turnTarget *= Quaternion.AngleAxis(signedDirection * _speed.Rotation, Vector3.up);
        }

        public void Jump(float multiplier = 1f)
        {
            _isJumping = true;
            _animator.Jump();
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * WorldValues.Gravity * multiplier);
            _turnTarget = _transform.rotation;
        }

        public void BoostedJump()
        {
            Jump(3f);
        }
    }
}
