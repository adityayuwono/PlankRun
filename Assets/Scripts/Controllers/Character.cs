using Assets.Scripts.Components;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class Character
    {
        public Action OnGameOver;
        public Action OnVictory;

        public IControlCharacter Control;

        protected Transform _finalGoal;
        protected Transform Transform;

        private Transform _plankRoot;
        private GameObject _plankTemplate;

        private Vector3 _lastPlankPosition;

        public Character(CharacterModel model, GameObject plankTemplate, Transform goal)
        {
            Name = model.GameObject.name;

            Transform = model.GameObject.transform;
            _plankTemplate = plankTemplate;

            _finalGoal = goal;
            while (_finalGoal.childCount > 0)
            {
                _finalGoal = _finalGoal.GetChild(0);
            }

            _plankRoot = Transform.Find("PlankRoot");

            var collisionHandler = model.GameObject.GetComponentInChildren<CollisionHandler>();
            collisionHandler.OnCollision += HandleVisionCollision;
        }

        public string Name { get; }
        public virtual int PlankCount { get; protected set; }
        public float DistanceToGoal { get { return Vector3.Distance(Transform.position, _finalGoal.position); } }

        public virtual void Update()
        {
            Control.Move();
        }

        public void AddPlank(Transform plank)
        {
            PlankCount++;

            plank.GetComponent<Collider>().enabled = false;
            plank.SetParent(_plankRoot, true);

            var sequence = DOTween.Sequence();
            var finalMoveTarget = Vector3.up * PlankCount * 0.2f;

            var firstMoveTarget = finalMoveTarget;
            firstMoveTarget.z += 1.5f;
            firstMoveTarget.y += 2f;

            sequence.Join(plank.DOLocalMove(firstMoveTarget, 0.1f));
            sequence.Join(plank.DOLocalRotate(Vector3.zero, 0.1f));

            sequence.Append(plank.DOLocalRotate(Vector3.right * 180, 0.1f, RotateMode.LocalAxisAdd));

            sequence.Append(plank.DOLocalMove(finalMoveTarget, 0.1f));
            sequence.Join(plank.DOLocalRotate(Vector3.right * 360, 0.1f, RotateMode.LocalAxisAdd));

            sequence.Play();
        }

        public void RemovePlank(Vector3 targetPosition)
        {
            PlankCount--;

            var plank = _plankRoot.GetChild(PlankCount);
            plank.SetParent(null);

            var sequence = DOTween.Sequence();
            var finalMoveTarget = targetPosition;

            var firstMoveTarget = plank.position;
            firstMoveTarget.z += 1.5f;
            firstMoveTarget.y += 2f;

            sequence.Join(plank.DOMove(firstMoveTarget, 0.1f));
            sequence.Join(plank.DOLocalRotate(Vector3.right * 360, 0.1f, RotateMode.LocalAxisAdd));

            sequence.Append(plank.DOLocalRotate(Vector3.right * 180, 0.1f, RotateMode.LocalAxisAdd));

            sequence.Append(plank.DOMove(finalMoveTarget, 0.1f));
            sequence.Join(plank.DOLocalRotate(Vector3.zero, 0.1f));

            sequence.AppendCallback(() => GameObject.Destroy(plank.gameObject));

            sequence.Play();
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
                RemovePlank(newPlankPosition);
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
