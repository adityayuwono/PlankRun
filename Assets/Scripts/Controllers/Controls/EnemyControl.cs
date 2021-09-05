using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;

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
            
        }

        public void Jump(float multiplier = 1)
        {
            
        }

        public void Move()
        {
            _animator.StartRunning();
        }

        public void Rotate(float signedDirection)
        {
            
        }
    }
}
