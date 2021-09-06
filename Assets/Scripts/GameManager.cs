using Assets.Scripts.Controllers;
using Assets.Scripts.Controllers.Inputs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Enums;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private StageModel _stage;

        [Header("UI")]
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private Text _rank;
        [SerializeField] private Text _leadership;

        private Character _player;
        private List<Enemy> _enemyControllers = new List<Enemy>();
        private IHandleInput InputHandler;

        private void Start()
        {
            CreatePlayer();
            CreateEnemies();

            _restartGameButton.onClick.AddListener(RestartGame);

            _onStateChanged.Add(GameState.Playing, OnPlayingStart);
            _onStateChanged.Add(GameState.GameOver, OnGameOver);
            _onStateChanged.Add(GameState.Victory, OnVictory);

            InputHandler = new Mouse();

            GoToState(GameState.StartMenu);
        }

        private void Update()
        {
            StateContainer?.Invoke();
        }

        private void CreatePlayer()
        {
            _player = new Player(_playerModel, _plankTemplate, _stage.FinalGoal);
            _player.OnGameOver += () => GoToState(GameState.GameOver);
            _player.OnVictory += () => GoToState(GameState.Victory);
        }

        private void CreateEnemies()
        {
            foreach (var enemy in _enemies)
            {
                var enemyController = new Enemy(enemy, _plankTemplate, _stage.EnemyGoalHierarchy);
                enemyController.OnVictory += () => GoToState(GameState.GameOver);
                _enemyControllers.Add(enemyController);
            }
        }

        #region States
        protected override void State_StartMenu()
        {
            InputHandler.ProcessForRotation((_) => { GoToState(GameState.Playing); });
        }

        protected override void State_Playing()
        {
            _player.Update();

            InputHandler.ProcessForRotation(_player.Control.Rotate);

            var playerRank = 1;

            foreach (var enemy in _enemyControllers)
            {
                if (enemy.DistanceToGoal < _player.DistanceToGoal)
                {
                    playerRank++;
                }

                enemy.Update();
            }

            _rank.text = playerRank.ToString();
        }

        protected override void State_GameOver()
        {

        }

        protected override void State_Victory()
        {

        }
        #endregion

        #region State Events
        private void OnPlayingStart()
        {
            foreach (var enemy in _enemyControllers)
            {
                enemy.Start();
            }
        }

        private void OnGameOver()
        {
            ShowLeadership();
            DisableEnemies();
        }

        private void OnVictory()
        {
            ShowLeadership();
            DisableEnemies();
        }

        private void ShowLeadership()
        {
            var characters = new List<Character>();
            characters.Add(_player);
            characters.AddRange(_enemyControllers);

            _leadership.text = string.Join("\n", characters.OrderBy(c => c.DistanceToGoal).Select(c => c.Name).ToArray());
        }

        private void DisableEnemies()
        {
            foreach (var enemy in _enemyControllers)
            {
                enemy.GameOver();
            }
        }
        #endregion

        private void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}