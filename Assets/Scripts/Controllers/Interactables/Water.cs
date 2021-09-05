using Assets.Scripts.Controllers;
using UnityEngine;

public class Water : MonoBehaviour, IInteractable
{
    public void Interact(Character character)
    {
        character.GameOver();
    }

    public void Vision(Character character)
    {

    }
}
