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
        protected Transform Transform;

        private Transform _plankRoot;
        private GameObject _plankTemplate;

        private Vector3 _lastPlankPosition;

        public Character(CharacterModel model, GameObject plankTemplate, Transform goal)
        {
            Transform = model.GameObject.transform;
            _plankTemplate = plankTemplate;
            Goal = goal;

            _plankRoot = Transform.Find("PlankRoot");

            var collisionHandler = model.GameObject.GetComponentInChildren<CollisionHandler>();
            collisionHandler.OnCollision += HandleVisionCollision;
        }

        public virtual int PlankCount { get; protected set; }
        public float DistanceToGoal { get { return Vector3.Distance(Transform.position, Goal.position); } }

        public virtual void Update()
        {
            Control.Move();
        }

        public void AddPlank(Transform plank)
        {
            PlankCount++;

            plank.GetComponent<Collider>().enabled = false;
            plank.SetParent(_plankRoot, false);
            plank.localPosition = Vector3.up * PlankCount * 0.2f;
        }

        public void RemovePlank()
        {
            PlankCount--;

            var plank = _plankRoot.GetChild(PlankCount);
            GameObject.Destroy(plank.gameObject);
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

        public void GameOver(bool isForced = false)
        {
            if (isForced || PlankCount <= 0)
            {
                Control = new DisabledControl();
                OnGameOver?.Invoke();
            }
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
                RemovePlank();
                _lastPlankPosition = newPlankPosition;

                var plank = GameObject.Instantiate(_plankTemplate);
                plank.transform.position = newPlankPosition;
                plank.transform.rotation = Transform.rotation;
            }
        }

        public virtual void HandleOnPath(RaycastHit hitInfo)
        {

        }
    }
}
