using UnityEngine;

namespace Assets.Scripts.Controllers.Interactables
{
    public class Plank : MonoBehaviour, IInteractable
    {
        private bool _isDisabled = false;

        public void Interact(Character character)
        {
            if (!_isDisabled)
            {
                _isDisabled = true;
                character.AddPlank();
                Destroy(gameObject);
            }
        }
    }
}