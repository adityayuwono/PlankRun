using System;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class CollisionHandler : MonoBehaviour
    {
        public Action<IInteractable> OnCollision;

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var interactable = hit.gameObject.GetComponent<IInteractable>();
            OnCollision?.Invoke(interactable);
        }
    }
}