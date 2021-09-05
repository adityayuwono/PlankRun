using System;
using UnityEngine;

public class VisionCollisionHandler : MonoBehaviour
{
    public Action<IInteractable> OnVision;

    private void OnCollisionEnter(Collision hit)
    {
        var interactable = hit.gameObject.GetComponent<IInteractable>();
        OnVision?.Invoke(interactable);
    }
}
