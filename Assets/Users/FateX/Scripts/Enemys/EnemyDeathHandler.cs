using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.Enemys
{
    public class EnemyDeathHandler: IInitializable, IDisposable
    {
        [Inject] private EnemyManager _enemyManager;
        [Inject] private ItemFactory _itemFactory;
        [Inject] private GameConfig _gameConfig;

        public void Initialize()
        {
            _enemyManager.OnEnemyDie += HandleEnemyDie;
        }

        public void Dispose()
        {
            _enemyManager.OnEnemyDie -= HandleEnemyDie;
        }

        private void HandleEnemyDie(EnemyBase obj)
        {
            var transformPosition = obj.transform.position;
            _itemFactory.SpawnXp(transformPosition + (Vector3)Random.insideUnitCircle);

            if (Random.Range(0f, 10000f) < _gameConfig.GameConfigData.DropCoinChance * 100f)
            {
                _itemFactory.SpawnCoin(transformPosition + (Vector3)Random.insideUnitCircle);
            }
        }
    }
}