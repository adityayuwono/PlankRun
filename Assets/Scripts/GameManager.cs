using Assets.Scripts.Controllers;
using Assets.Scripts.Controllers.Inputs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : GameStateManager
    {
        [SerializeField] private CharacterModel _character;
        [SerializeField] private GameObject _plankTemplate;
        [SerializeField] private Button _restartGameButton;

        private Character _characterController;
        private IHandleInput InputHandler;

        private void Start()
        {
            _characterController = new Character(_character, _plankTemplate);
            _characterController.OnGameOver += () => GoToState(GameState.GameOver);

            InputHandler = new Mouse();
            _restartGameButton.onClick.AddListener(RestartGame);

            GoToState(GameState.StartMenu);
        }

        private void Update()
        {
            StateContainer?.Invoke();
        }

        protected override void State_StartMenu()
        {
            InputHandler.ProcessForRotation((_) => { GoToState(GameState.Playing); });
        }

        protected override void State_Playing()
        {
            _characterController.Control.Move();

            InputHandler.ProcessForRotation(_characterController.Control.Rotate);
        }

        protected override void State_GameOver()
        {

        }

        private void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}