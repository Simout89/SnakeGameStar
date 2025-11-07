using System;
using UnityEngine;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class GamePlaySceneEntryPoint: IInitializable
    {
        [Inject] private IInputService _inputService;
        
        [Inject] private GameTimer _gameTimer;
        [Inject] private SnakeSpawner _snakeSpawner;
        [Inject] private EnemyManager _enemyManager;

        public void Initialize()
        {
            Debug.Log("W");

            Snake snake = _snakeSpawner.SpawnSnake();
            
            _enemyManager.SetSnake(snake);
            
            _gameTimer.StartTimer();
        }
    }
}