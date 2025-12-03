using System;
using Lean.Pool;
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
        [Inject] private PlayerStats _playerStats;
        
        public int TotalEnemyDie { get; private set; }

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
            TotalEnemyDie++;
            
            var transformPosition = obj.transform.position;
            if(obj.GetData().OverrideLootXP == null || obj.GetData().OverrideLootXP.Length == 0)
                _itemFactory.SpawnXp(transformPosition + (Vector3)Random.insideUnitCircle / 2);
            else
            {
                foreach (var xpItem in obj.GetData().OverrideLootXP)
                {
                    _itemFactory.SpawnXp(transformPosition + (Vector3)Random.insideUnitCircle / 2, xpItem);
                }
            }

            if (Random.Range(0f, 10000f) < (_gameConfig.GameConfigData.DropCoinChance + _playerStats.CoinDropChance.Sum) * 100f)
            {
                _itemFactory.SpawnCoin(transformPosition + (Vector3)Random.insideUnitCircle / 2);
            }

            if (TotalEnemyDie % (100 / _gameConfig.GameConfigData.MagnetDropChance) == 0)
            {
                _itemFactory.SpawnMagnet(transformPosition + (Vector3)Random.insideUnitCircle / 2);
            }

            if (obj.GetData().ItemLoot != null)
            {
                foreach (var VARIABLE in obj.GetData().ItemLoot)
                {
                    LeanPool.Spawn(VARIABLE, obj.transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                }
            }
        }
    }
}