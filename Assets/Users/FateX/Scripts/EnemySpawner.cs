using System;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;
        
        [SerializeField] private Enemy enemyPrefab;

        private void Awake()
        {
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            Enemy enemy = Instantiate(enemyPrefab);
            
            _enemyManager.AddEnemy(enemy);
        }
    }
}