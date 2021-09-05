using Assets.Scripts.Models.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class StateUIModel
    {
        public GameState GameState;
        public List<GameObject> UIRoots;

        public void SetActive(bool isActive)
        {
            foreach(var uiRoot in UIRoots)
            {
                uiRoot.SetActive(isActive);
            }
        }
    }
}
