using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class CharacterModel
    {
        public GameObject GameObject;
        public Text PlankInformation;
        public SpeedModel Speed;
        public float JumpHeight;
    }
}
