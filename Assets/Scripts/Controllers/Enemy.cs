using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.AI;

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
        
        private bool _isOnShortcut;

        private IControlCharacter _enemyControl;
        private IControlCharacter _shortcutControl;
        private bool _isGoingToInteractables;

        public Enemy(CharacterModel model, GameObject plankTemplate, Transform goal) :
            base(model, plankTemplate, goal)
        {
            _enemyControl = new EnemyControl(model, this);
            _shortcutControl = new PlayerControl(model, this);

            _agent = model.GameObject.GetComponent<NavMeshAgent>();
            _agent.speed = model.Speed.Movement;
            _goal = goal;

            _collisionHandler = model.GameObject.GetComponentInChildren<VisionCollisionHandler>();

            _shortcutChecker = Transform.Find("ShortcutChecker");
            _shortcutCollider = Transform.Find("ShortcutCollider");
            _shortcutCollider.GetComponent<ShortcutCollisionCheck>().Enemy = this;

            UseNavigationStrategy();
        }

        public void Start()
        {
            _agent.SetDestination(_goal.position);

            _collisionHandler.OnVision += HandleVisionCollision;
        }

        public override void Update()
        {
            base.Update();

            _movementStrategy?.Invoke();
        }

        public void DoTargetInteractable(Transform transform)
        {
            _interactStrategy?.Invoke(transform);
        }

        public void DoShortcut()
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
            var isOnNavMesh = NavMesh.SamplePosition(Transform.position, out hit, 1.6f, NavMesh.AllAreas);

            if (_isOnShortcut && isOnNavMesh)
            {
                _isOnShortcut = false;

                UseNavigationStrategy();
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
                if (_isGoingToInteractables)
                {
                    _isGoingToInteractables = false;
                }
                else
                {
                    var nextGoal = _goal.GetChild(0);
                    if (nextGoal != null)
                    {
                        _goal = nextGoal;
                    }
                }

                UpdateDestination(_goal);
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
                UseShortcutStrategy();
            }
        }

        private void UseShortcutStrategy()
        {
            _agent.enabled = false;

            Control = _shortcutControl;
            _movementStrategy = TakeShortcut;
            _shortcutStrategy = null;
            _interactStrategy = null;

            _shortcutCollider.SetParent(null);
        }

        private void UseNavigationStrategy()
        {
            _agent.enabled = true;

            Control = _enemyControl;
            _movementStrategy = NavigateToGoal;
            _shortcutStrategy = CheckPossibleShortcut;
            _interactStrategy = GoToVisibleInteractables;

            _shortcutCollider.SetParent(Transform);
        }

        private void GoToVisibleInteractables(Transform transform)
        {
            if (UnityEngine.Random.Range(0, 10) < 1 && PlankCount < 30)
            {
                _isGoingToInteractables = true;
                UpdateDestination(transform);
            }
        }

        private void UpdateDestination(Transform destination)
        {
            _agent.SetDestination(destination.position);
        }
    }
}
