using Assets.Scripts.Models.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class StateUIModel
    {
        public GameState GameState;
        public GameObject UIRoot;
    }
}
