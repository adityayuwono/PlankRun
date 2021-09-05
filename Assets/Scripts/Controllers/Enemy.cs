using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class Enemy : Character
    {
        private NavMeshAgent _agent;
        private Transform _goal;
        private VisionCollisionHandler _collisionHandler;
        private Transform _shortcutChecker;

        private Transform _shortcutCollider;

        private Action _movementStrategy;
        private Action _shortcutStrategy;
        private Action<Transform> _interactStrategy;
        private CharacterModel _model;

        private bool _isOnShortcut;

        public Enemy(CharacterModel model, GameObject plankTemplate, Transform goal) :
            base(model, plankTemplate, goal)
        {
            _model = model;
            Control = new EnemyControl(model, this);

            _agent = model.GameObject.GetComponent<NavMeshAgent>();
            _agent.speed = model.Speed.Movement;
            _goal = goal;

            _collisionHandler = model.GameObject.GetComponentInChildren<VisionCollisionHandler>();

            _shortcutChecker = model.GameObject.transform.Find("ShortcutChecker");
            _shortcutCollider = model.GameObject.transform.Find("ShortcutCollider");
            _shortcutCollider.GetComponent<ShortcutCollisionCheck>().Enemy = this;

            _movementStrategy = NavigateToGoal;
            _shortcutStrategy = CheckPossibleShortcut;
            _interactStrategy = GoToVisibleInteractables;
        }

        public void Start()
        {
            _agent.SetDestination(_goal.position);

            _collisionHandler.OnVision += HandleVisionCollision;
        }

        public override void Update()
        {
            base.Update();

            _movementStrategy();
        }

        public void TargetInteractable(Transform transform)
        {
            _interactStrategy?.Invoke(transform);
        }

        public void TryTakingShortcut()
        {
            _shortcutStrategy?.Invoke();
        }

        protected override void DropPlank(RaycastHit hitInfo)
        {
            _isOnShortcut = true;

            base.DropPlank(hitInfo);
        }

        public override void HandleOnPath(RaycastHit hitInfo)
        {
            var hit = new NavMeshHit();
            var isOnNavMesh = NavMesh.SamplePosition(_model.GameObject.transform.position, out hit, 1.6f, NavMesh.AllAreas);

            if (_isOnShortcut && isOnNavMesh)
            {
                _isOnShortcut = false;

                GoBackToPathNavigation();
            }
        }

        private void HandleVisionCollision(IInteractable interactable)
        {
            interactable?.Vision(this);
        }

        private void NavigateToGoal()
        {
            if (_agent.ReachedDestination())
            {
                _agent.SetDestination(_goal.position);
            }

            _shortcutChecker.LookAt(_goal);
            var scale = _shortcutChecker.localScale;
            scale.z = PlankCount * 1.5f;
            _shortcutChecker.localScale = scale;

            var tipPosition = _shortcutChecker.GetComponentInChildren<MeshRenderer>().bounds.max;
            _shortcutCollider.position = tipPosition;
        }

        private void TakeShortcut()
        {
            _agent.transform.LookAt(_goal);
        }

        private void CheckPossibleShortcut()
        {
            if (PlankCount >= 15)
            {
                _agent.enabled = false;

                Control = new PlayerControl(_model, this);
                _movementStrategy = TakeShortcut;
                _shortcutStrategy = null;
                _interactStrategy = null;
                _shortcutCollider.SetParent(null);
            }
        }

        private void GoBackToPathNavigation()
        {
            _agent.enabled = true;

            Control = new EnemyControl(_model, this);
            _movementStrategy = NavigateToGoal;
            _shortcutStrategy = CheckPossibleShortcut;
            _interactStrategy = GoToVisibleInteractables;

            _shortcutCollider.SetParent(_model.GameObject.transform);
        }

        private void GoToVisibleInteractables(Transform transform)
        {
            if (UnityEngine.Random.Range(0, 10) < 1 && PlankCount < 30)
            {
                _agent.SetDestination(transform.position);
            }
        }
    }
}
