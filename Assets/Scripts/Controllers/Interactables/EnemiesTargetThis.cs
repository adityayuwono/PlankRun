using UnityEngine;

namespace Assets.Scripts.Controllers.Interactables
{
    public class EnemiesTargetThis : MonoBehaviour, IInteractable
    {
        public virtual void Interact(Character character)
        {

        }

        public virtual void Vision(Character character)
        {
            var enemy = character as Enemy;
            enemy?.TargetInteractable(transform);
        }
    }
}
