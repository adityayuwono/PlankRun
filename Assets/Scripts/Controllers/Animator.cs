using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class Animator
    {
        private UnityEngine.Animator _animator;

        public Animator(GameObject gameObject, float movementSpeed)
        {
            _animator = gameObject.GetComponentInChildren<UnityEngine.Animator>();
            _animator.SetFloat("RunningSpeed", movementSpeed/8f);
        }

        public void StartRunning()
        {
            _animator.SetTrigger("Run");
        }

        public void Jump()
        {
            _animator.SetTrigger("Jump");
        }

        public void Land()
        {
            _animator.SetTrigger("Land");
        }
    }
}
