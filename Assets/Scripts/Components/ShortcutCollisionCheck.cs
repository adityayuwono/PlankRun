using Assets.Scripts.Controllers;
using UnityEngine;

public class ShortcutCollisionCheck : MonoBehaviour
{
    public Enemy Enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Path")
        {
            Enemy.TryTakingShortcut();
        }
    }
}
