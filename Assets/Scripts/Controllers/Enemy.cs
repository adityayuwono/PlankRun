using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Controllers
{
    public class Enemy : Character
    {
        private NavMeshAgent _agent;
        private Transform _goal;
        private VisionCollisionHandler _collisionHandler;

        public Enemy(CharacterModel model, GameObject plankTemplate, Transform goal) : 
            base(model, plankTemplate, goal)
        {
            Control = new EnemyControl(model, this);

            _agent = model.GameObject.GetComponent<NavMeshAgent>();
            _agent.speed = model.Speed.Movement;
            _goal = goal;

            _collisionHandler = model.GameObject.GetComponentInChildren<VisionCollisionHandler>();
        }

        public void Start()
        {
            _agent.SetDestination(_goal.position);

            _collisionHandler.OnVision += HandleVisionCollision;
        }

        public void Update()
        {
            Control.Move();

            if (_agent.ReachedDestination())
            {
                _agent.SetDestination(_goal.position);
            }
        }

        public void TargetInteractable(Transform transform)
        {
            if (UnityEngine.Random.Range(0, 10) < 1 && PlankCount < 30)
            {
                _agent.SetDestination(transform.position);
            }
        }

        private void HandleVisionCollision(IInteractable interactable)
        {
            interactable?.Vision(this);
        }
    }
}
