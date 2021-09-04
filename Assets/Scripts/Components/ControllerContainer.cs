using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ControllerContainer : MonoBehaviour
    {
        public Controller Controller { get; private set; }
    }
}
