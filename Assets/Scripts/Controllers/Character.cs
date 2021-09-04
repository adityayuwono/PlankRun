using Assets.Scripts.Components;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class Character
    {
        public Action OnGameOver;

        public IControlCharacter Control;

        private Transform _transform;
        private Text _plankInformation;
        private string _plankInformationFormat;
        private GameObject _plankTemplate;

        public Character(CharacterModel model, GameObject plankTemplate)
        {
            _transform = model.GameObject.transform;
            _plankInformation = model.PlankInformation;
            _plankInformationFormat = _plankInformation.text;
            _plankTemplate = plankTemplate;

            Control = new Control(model, this);

            var collisionHandler = model.GameObject.GetComponent<CollisionHandler>();
            collisionHandler.OnCollision += HandleCollision;
        }

        private int _plankCount;
        private Vector3 _lastPlankPosition;

        public int PlankCount
        {
            get { return _plankCount; }
            private set
            {
                _plankCount = value;
                _plankInformation.text = string.Format(_plankInformationFormat, _plankCount);
            }
        }

        public void AddPlank()
        {
            PlankCount++;
        }

        private void HandleCollision(IInteractable collidee)
        {
            collidee?.Interact(this);
        }

        public void HandleNearWater(RaycastHit hitInfo)
        {
            if (PlankCount > 0)
            {
                DropPlank(hitInfo);
            }
            else
            {
                Control.Jump();
            }
        }

        public void GameOver()
        {
            Control = new DisabledControl();
            OnGameOver?.Invoke();
        }

        private void DropPlank(RaycastHit hitInfo)
        {
            var newPlankPosition = hitInfo.point;
            var distance = Vector3.Distance(newPlankPosition, _lastPlankPosition);

            if (distance >= 1.5f)
            {
                PlankCount--;

                var plank = GameObject.Instantiate(_plankTemplate);
                plank.transform.position = newPlankPosition;
                plank.transform.rotation = _transform.rotation;

                _lastPlankPosition = newPlankPosition;
            }
        }
    }
}
