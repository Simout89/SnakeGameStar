using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class SnakeSpawner: MonoBehaviour
    {
        [Inject] private IInputService _inputService;
        
        [FormerlySerializedAs("_snakePrefab")] [SerializeField] private SnakeController snakeControllerPrefab;
        [SerializeField] private Transform spawnPoint;

        public SnakeController SpawnSnake()
        {
            SnakeController snakeController = Instantiate(snakeControllerPrefab);
            snakeController.Init(_inputService);
            snakeController.transform.position = spawnPoint.position;
            return snakeController;
        }
    }
}