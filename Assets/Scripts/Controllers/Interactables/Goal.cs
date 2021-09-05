using UnityEngine;

namespace Assets.Scripts.Controllers.Interactables
{
    public class Goal : MonoBehaviour, IInteractable
    {
        public void Interact(Character character)
        {
            character.Victory();
        }

        public void Vision(Character character)
        {

        }
    }
}
