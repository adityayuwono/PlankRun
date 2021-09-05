using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using System;

namespace Assets.Scripts.Controllers
{
    public class EnemyControl : IControlCharacter
    {
        private Animator _animator;

        public EnemyControl(CharacterModel model, Character character)
        {
            _animator = new Animator(model.GameObject, model.Speed.Movement);
        }

        public void BoostedJump()
        {
            throw new NotImplementedException();
        }

        public void Jump(float multiplier = 1)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            _animator.StartRunning();
        }

        public void Rotate(float signedDirection)
        {
            throw new NotImplementedException();
        }
    }
}
