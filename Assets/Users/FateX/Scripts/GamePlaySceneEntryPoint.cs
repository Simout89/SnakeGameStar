using System;
using UnityEngine;
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

        public void Initialize()
        {
            Debug.Log("W");

            Snake snake = _snakeSpawner.SpawnSnake();
            
            _enemyManager.SetSnake(snake);

            WaveData waveData = Resources.LoadAll<WaveData>("Data/Waves")[0];
            
            _enemySpawnDirector.SetWaveData(waveData);
            
            _gameTimer.StartTimer(waveData.TotalTime);
        }
    }
}