using System;
using UnityEngine;
using Users.FateX.Scripts.Cards;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Data.WaveData;
using Users.FateX.Scripts.Enemy;
using Users.FateX.Scripts.Services;
using Users.FateX.Scripts.View;
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
        [Inject] private ItemFactory _itemFactory;
        [Inject] private CameraController _cameraController;
        
        [Inject] private CardMenuController _cardMenuController;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private HealthView _healthView;
        [Inject] private DeathHandler _deathHandler;

        
        public void Initialize()
        {

            // УРовень
            
            SnakeController snakeController = _snakeSpawner.SpawnSnake();

            _cameraController.SetTarget(snakeController.transform);

            _gameContext.Init(snakeController);

            _collectableHandler.SetSnakeInteraction(snakeController.GetComponent<SnakeInteraction>());

            _enemyManager.SetSnake(snakeController);

            var snakeHealth = snakeController.GetComponent<SnakeHealth>();
            _healthView.SetSnakeHealth(snakeHealth);
            
            _deathHandler.SetSnakeHealth(snakeHealth);
            
            WaveData waveData = Resources.LoadAll<WaveData>("Data/Waves")[0];
            
            _enemySpawnDirector.SetWaveData(waveData);

            _gameTimer.StartTimer(waveData.TotalTime);

            _cardMenuController.SpawnRandomCards();

            _gameStateManager.PushState(GameStates.CardMenu);
        }
    }
}