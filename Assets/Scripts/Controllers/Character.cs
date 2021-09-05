using Assets.Scripts.Components;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class Character
    {
        public Action OnGameOver;
        public Action OnVictory;

        public IControlCharacter Control;

        protected Transform Goal;

        private Transform _transform;
        private GameObject _plankTemplate;

        private Vector3 _lastPlankPosition;

        public Character(CharacterModel model, GameObject plankTemplate, Transform goal)
        {
            _transform = model.GameObject.transform;
            _plankTemplate = plankTemplate;
            Goal = goal;

            var collisionHandler = model.GameObject.GetComponentInChildren<CollisionHandler>();
            collisionHandler.OnCollision += HandleVisionCollision;
        }

        public virtual int PlankCount { get; protected set; }
        public float DistanceToGoal { get { return Vector3.Distance(_transform.position, Goal.position); } }

        public void AddPlank()
        {
            PlankCount++;
        }

        private void HandleVisionCollision(IInteractable collidee)
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

        public void Victory()
        {
            Control = new DisabledControl();
            OnVictory?.Invoke();
        }

        protected virtual void DropPlank(RaycastHit hitInfo)
        {
            var newPlankPosition = hitInfo.point;
            var distance = Vector3.Distance(newPlankPosition, _lastPlankPosition);

            if (distance >= 1.5f)
            {
                PlankCount--;
                _lastPlankPosition = newPlankPosition;

                var plank = GameObject.Instantiate(_plankTemplate);
                plank.transform.position = newPlankPosition;
                plank.transform.rotation = _transform.rotation;
            }
        }

        public virtual void HandleOnPath(RaycastHit hitInfo)
        {

        }
    }
}
