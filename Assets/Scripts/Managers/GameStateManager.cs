using Assets.Scripts.Models;
using Assets.Scripts.Models.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private StateUIModel _activeUI;
        [SerializeField] private List<StateUIModel> _uis = new List<StateUIModel>();

        protected Dictionary<GameState, Action> _stateActions = new Dictionary<GameState, Action>();
        protected Dictionary<GameState, Action> _onStateChanged = new Dictionary<GameState, Action>();

        protected Action StateContainer;

        private void Awake()
        {
            _stateActions.Add(GameState.StartMenu, State_StartMenu);
            _stateActions.Add(GameState.Playing, State_Playing);
            _stateActions.Add(GameState.GameOver, State_GameOver);
            _stateActions.Add(GameState.Victory, State_Victory);
        }

        protected void GoToState(GameState newState)
        {
            UpdateUI(newState);
            InvokeOnStateChangedEvent(newState);
            UpdateStateDelegate(newState);
            
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

        protected virtual void State_Victory()
        {
        }

        private void UpdateUI(GameState newState)
        {
            _activeUI.SetActive(false);
            _activeUI = _uis.Find(s => s.GameState == newState);
            _activeUI.SetActive(true);
        }

        private void InvokeOnStateChangedEvent(GameState newState)
        {
            if (_onStateChanged.ContainsKey(newState))
            {
                _onStateChanged[newState]();
            }
        }

        private void UpdateStateDelegate(GameState newState)
        {
            StateContainer = _stateActions[newState];
        }
    }
}
