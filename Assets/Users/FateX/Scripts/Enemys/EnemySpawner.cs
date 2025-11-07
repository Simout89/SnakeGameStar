using System;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Entity;
using Zenject;

namespace Users.FateX.Scripts
{
    public class EnemySpawner: MonoBehaviour
    {
        [Inject] private EnemySpawnArea _enemySpawnArea;
        [Inject] private EnemyManager _enemyManager;

        public void SpawnEnemy(EnemyBase enemyPrefab)
        {
            EnemyBase enemy = LeanPool.Spawn(enemyPrefab);

            enemy.transform.position = _enemySpawnArea.GetRandomPositionOnBorder();
            
            _enemyManager.AddEnemy(enemy);
        }
    }
}