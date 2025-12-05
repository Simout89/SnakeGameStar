using UnityEngine;
using Users.FateX.Scripts.Enemy;
using Zenject;
using System;
using System.Collections.Generic;
using Users.FateX.Scripts.View;

namespace Users.FateX.Scripts.Trial
{
    public class TrialDirector
    {
        [Inject] private EnemyFactory enemyFactory;
        [Inject] private EnemySpawnDirector _enemySpawnDirector;
        [Inject] private ItemFactory _itemFactory;
        [Inject] private MessageDisplayView _messageDisplayView;
        
        public void OnTowerCaptured(TrialTower tower)
        {
            var enemies = new List<EnemyBase>();
            var handlers = new Dictionary<EnemyBase, Action<EnemyBase>>();
            _messageDisplayView.ShowText("Лудоманы вызваны");
            
            for (int i = 0; i < 5; i++)
            {
                var enemy = enemyFactory.SpawnEliteEnemy(_enemySpawnDirector.GetEnemiesNWavesAhead(2)[0]);
                enemies.Add(enemy);
                
                Action<EnemyBase> handler = (_) => OnEnemyDied(enemy, enemies, handlers, tower);
                handlers[enemy] = handler;
                enemy.OnDie += handler;
            }
        }

        private void OnEnemyDied(EnemyBase deadEnemy, List<EnemyBase> enemyPack, 
            Dictionary<EnemyBase, Action<EnemyBase>> handlers, TrialTower tower)
        {
            if (handlers.TryGetValue(deadEnemy, out var handler))
            {
                deadEnemy.OnDie -= handler;
                handlers.Remove(deadEnemy);
            }
            
            enemyPack.Remove(deadEnemy);
            
            if (enemyPack.Count == 0)
            {
                OnPackCleared(tower, deadEnemy.transform.position);
            }
        }

        private void OnPackCleared(TrialTower tower, Vector3 lastEnemyPosition)
        {
            Debug.Log($"Башня {tower.name}: все враги зачищены! Последний был в {lastEnemyPosition}");
            _itemFactory.SpawnGamblingItem(lastEnemyPosition);
        }
    }
}