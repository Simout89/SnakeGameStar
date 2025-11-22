using System;
using UnityEngine;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Data.WaveData;
using Users.FateX.Scripts.Enemy;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class GamePlaySceneEntryPoint: IInitializable
    {
        [Inject] private IInputService _inputService;

        [Inject] private EnemySpawnDirector _enemySpawnDirector;
        [Inject] private GameTimer _gameTimer;
        [Inject] private SnakeSpawner _snakeSpawner;
        [Inject] private EnemyManager _enemyManager;
        [Inject] private GameContext _gameContext;
        [Inject] private CollectableHandler _collectableHandler;
        [Inject] private ExperienceFactory _experienceFactory;
        
        public void Initialize()
        {
            Debug.Log("W");
            
            SnakeController snakeController = _snakeSpawner.SpawnSnake();
            
            _gameContext.Init(snakeController);
            
            _collectableHandler.SetSnakeInteraction(snakeController.GetComponent<SnakeInteraction>());
            
            _enemyManager.SetSnake(snakeController);

            WaveData waveData = Resources.LoadAll<WaveData>("Data/Waves")[0];
            
            _experienceFactory.SetPrefab(Resources.LoadAll<XpItem>("Prefabs")[0]);
            
            _enemySpawnDirector.SetWaveData(waveData);
            
            _gameTimer.StartTimer(waveData.TotalTime);
        }
    }
}