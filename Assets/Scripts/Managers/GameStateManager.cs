using Assets.Scripts.Models;
using Assets.Scripts.Models.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private GameObject _activeUI;
        [SerializeField] private List<StateUIModel> _uis = new List<StateUIModel>();

        protected Dictionary<GameState, Action> _stateActions = new Dictionary<GameState, Action>();

        protected Action StateContainer;

        private void Awake()
        {
            _stateActions.Add(GameState.StartMenu, State_StartMenu);
            _stateActions.Add(GameState.Playing, State_Playing);
            _stateActions.Add(GameState.GameOver, State_GameOver);
        }

        protected void GoToState(GameState newState)
        {
            _activeUI.SetActive(false);
            _activeUI = _uis.Find(s => s.GameState == newState).UIRoot;
            _activeUI.SetActive(true);
            StateContainer = _stateActions[newState];
        }

        protected virtual void State_StartMenu()
        {

        }

        protected virtual void State_Playing()
        {

        }

        protected virtual void State_GameOver()
        {

        }
    }
}
