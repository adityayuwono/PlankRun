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
        [SerializeField] private CharacterModel _playerModel;
        [SerializeField] private CharacterModel[] _enemies;
        [SerializeField] private GameObject _plankTemplate;
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private Transform _goal;

        private Character _player;
        private List<Enemy> _enemyControllers = new List<Enemy>();
        private IHandleInput InputHandler;

        private void Start()
        {
            CreatePlayer();
            CreateEnemies();

            _restartGameButton.onClick.AddListener(RestartGame);

            _onStateChanged.Add(GameState.Playing, OnPlayingStart);

            InputHandler = new Mouse();

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
            _player.Control.Move();

            InputHandler.ProcessForRotation(_player.Control.Rotate);

            foreach(var enemy in _enemyControllers)
            {
                enemy.Update();
            }
        }

        protected override void State_GameOver()
        {

        }

        protected override void State_Victory()
        {

        }

        private void CreatePlayer()
        {
            _player = new Player(_playerModel, _plankTemplate, _goal);
            _player.OnGameOver += () => GoToState(GameState.GameOver);
            _player.OnVictory += () => GoToState(GameState.Victory);
        }

        private void CreateEnemies()
        {
            foreach (var enemy in _enemies)
            {
                var enemyController = new Enemy(enemy, _plankTemplate, _goal);
                enemyController.OnVictory += () => GoToState(GameState.GameOver);
                _enemyControllers.Add(enemyController);
            }
        }

        private void OnPlayingStart()
        {
            foreach (var enemy in _enemyControllers)
            {
                enemy.Start();
            }
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}