using UnityEngine;

namespace Assets.Scripts.Controllers.Interactables
{
    public class JumpBooster : MonoBehaviour, IInteractable
    {
        public void Interact(Character character)
        {
            character.Control.BoostedJump();
        }
    }
}