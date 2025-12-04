using System;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Entity;
using Zenject;

namespace Users.FateX.Scripts
{
    public class EnemyFactory
    {
        [Inject] private EnemySpawnArea _enemySpawnArea;
        [Inject] private EnemyManager _enemyManager;
        [Inject] private GameConfig _gameConfig;

        public void SpawnEnemy(EnemyBase enemyPrefab)
        {
            EnemyBase enemy = LeanPool.Spawn(enemyPrefab);

            enemy.transform.position = _enemySpawnArea.GetRandomPositionOnBorder();
            
            _enemyManager.AddEnemy(enemy);
        }
        
        public void SpawnFinalEnemy(float statsMultiply)
        {
            EnemyBase enemy = LeanPool.Spawn(_gameConfig.GameConfigData.InfinityEnemy);
            
            enemy.MultiplyStats(statsMultiply);
            
            enemy.transform.position = _enemySpawnArea.GetRandomPositionOnBorder();
            
            _enemyManager.AddEnemy(enemy);
        }

        public EnemyBase SpawnEliteEnemy(EnemyBase enemyPrefab)
        {
            EnemyBase enemy = LeanPool.Spawn(enemyPrefab);

            enemy.transform.position = _enemySpawnArea.GetRandomPositionOnBorder();
            
            enemy.MultiplyStats(3);

            enemy.SpriteRenderer.material = _gameConfig.GameConfigData.EnemyMaterials.EliteEnemy;
            
            _enemyManager.AddEnemy(enemy);

            return enemy;
        }
    }
}