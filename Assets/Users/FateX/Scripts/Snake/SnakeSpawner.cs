using UnityEngine;
using UnityEngine.Serialization;
using Users.FateX.Scripts.Services;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class SnakeSpawner: MonoBehaviour
    {
        [Inject] private IInputService _inputService;
        [Inject] private PlayerStats _playerStats;
        [Inject] private DiContainer _diContainer;
        
        [FormerlySerializedAs("_snakePrefab")] [SerializeField] private SnakeController snakeControllerPrefab;
        [SerializeField] private Transform spawnPoint;

        public SnakeController SpawnSnake()
        {
            SnakeController snakeController = Instantiate(snakeControllerPrefab);
            _diContainer.Inject(snakeController);
            snakeController.Init(_inputService, _playerStats);
            snakeController.transform.position = spawnPoint.position;
            return snakeController;
        }
    }
}