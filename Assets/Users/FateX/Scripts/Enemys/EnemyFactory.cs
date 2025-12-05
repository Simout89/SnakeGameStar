using System;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Entity;
using Zenject;
using Random = UnityEngine.Random;

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

            if (enemyPrefab.EnemyData.OverrideMaterial == null)
            {
                enemy.SpriteRenderer.material = _gameConfig.GameConfigData.EnemyMaterials.DefaultMaterial;
                enemy.SpriteRenderer.material.DisableKeyword("OUTBASE_ON");
                enemy.SpriteRenderer.material.DisableKeyword("OUTTEX_ON");
            }
            else
            {
                enemy.SpriteRenderer.material = enemyPrefab.EnemyData.OverrideMaterial;
            }
            
            _enemyManager.AddEnemy(enemy);
        }
        
        public void SpawnFinalEnemy(float statsMultiply)
        {
            EnemyBase enemy = LeanPool.Spawn(_gameConfig.GameConfigData.InfinityEnemy);
            
            enemy.MultiplyStats(statsMultiply);
            
            enemy.transform.position = _enemySpawnArea.GetRandomPositionOnBorder();
            
            _enemyManager.AddEnemy(enemy);
        }

        public EnemyBase SpawnGamblingEnemy(EnemyBase enemyPrefab)
        {
            EnemyBase enemy = LeanPool.Spawn(enemyPrefab);

            enemy.transform.position = _enemySpawnArea.GetRandomPositionOnBorder();
            
            enemy.MultiplyStats(3);
            
            enemy.SpriteRenderer.material.EnableKeyword("OUTBASE_ON");
            enemy.SpriteRenderer.material.EnableKeyword("OUTTEX_ON");
            enemy.SpriteRenderer.material.SetColor("_OutlineColor", Color.white);

            
            _enemyManager.AddEnemy(enemy);

            return enemy;
        }

        public EnemyBase SpawnGoldRushEnemy(EnemyBase enemyPrefab)
        {
            EnemyBase enemy = LeanPool.Spawn(enemyPrefab);

            enemy.transform.position = _enemySpawnArea.GetRandomPositionOnBorder();
            
            enemy.MultiplyStats(0.1f);

            enemy.CoinDropCount = Random.Range(1, 2);
            
            enemy.SpriteRenderer.material.EnableKeyword("OUTBASE_ON");
            enemy.SpriteRenderer.material.SetColor("_OutlineColor", Color.yellow);
            
            _enemyManager.AddEnemy(enemy);

            return enemy;
        }
    }
}