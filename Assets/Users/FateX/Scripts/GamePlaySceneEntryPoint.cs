using System;
using UnityEngine;
using Users.FateX.Scripts.Cards;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Data.WaveData;
using Users.FateX.Scripts.Enemy;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class GamePlaySceneEntryPoint : IInitializable
    {
        [Inject] private IInputService _inputService;

        [Inject] private EnemySpawnDirector _enemySpawnDirector;
        [Inject] private GameTimer _gameTimer;
        [Inject] private SnakeSpawner _snakeSpawner;
        [Inject] private EnemyManager _enemyManager;
        [Inject] private GameContext _gameContext;
        [Inject] private CollectableHandler _collectableHandler;
        [Inject] private ExperienceFactory _experienceFactory;
        [Inject] private CameraController _cameraController;

        [Inject] private GameConfig _gameConfig;

        [Inject] private CardMenuController _cardMenuController;
        [Inject] private GameStateManager _gameStateManager;

        public void Initialize()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
    Application.targetFrameRate = 90;
#endif


            Debug.Log("W");

            SnakeController snakeController = _snakeSpawner.SpawnSnake();

            _cameraController.SetTarget(snakeController.transform);

            _gameContext.Init(snakeController);

            _collectableHandler.SetSnakeInteraction(snakeController.GetComponent<SnakeInteraction>());

            _enemyManager.SetSnake(snakeController);

            _gameConfig.SetConfig(Resources.LoadAll<GameConfigData>("Data")[0]);

            WaveData waveData = Resources.LoadAll<WaveData>("Data/Waves")[0];

            _experienceFactory.SetPrefab(_gameConfig.GameConfigData.XpPrefab);

            _enemySpawnDirector.SetWaveData(waveData);

            _gameTimer.StartTimer(waveData.TotalTime);

            _cardMenuController.SpawnRandomCards();

            _gameStateManager.ChangeState(GameStates.CardMenu);
        }
    }
}