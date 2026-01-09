using UnityEngine;
using Users.FateX.Scripts.Enemy;
using Zenject;
using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Users.FateX.Scripts.Analytics.Events;
using Users.FateX.Scripts.View;

namespace Users.FateX.Scripts.Trial
{
    public class TrialWaveDirector
    {
        [Inject] private EnemyFactory enemyFactory;
        [Inject] private EnemySpawnDirector _enemySpawnDirector;
        [Inject] private ItemFactory _itemFactory;
        [Inject] private MessageDisplayView _messageDisplayView;
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        [Inject] private ArrowView _arrowView;
        [Inject] private GameConfig _gameConfig;

        private bool firstTowerCaptured = false;
        
        public void OnTowerCaptured(TrialTower tower)
        {
            var enemies = new List<EnemyBase>();
            var handlers = new Dictionary<EnemyBase, Action<EnemyBase>>();

            int spawnCount = 5;


            if (!firstTowerCaptured)
            {
                AnalyticsService.Instance.RecordEvent(
                    new OnCaptureFirstTower()
                );

                firstTowerCaptured = true;
            }
            
            
            _arrowView.StopTracking(tower.gameObject);
            
            switch (tower.TrialTowerType)
            {
                case TrialTowerType.Gambling:
                {
                    _messageDisplayView.ShowText(_gameConfig.GameConfigData.LocalizationData.TrialTowerGambler.GetLocalizedString(), Color.purple);
                    spawnCount = 5;
                } break;
                case TrialTowerType.GoldRush:
                {
                    _messageDisplayView.ShowText(_gameConfig.GameConfigData.LocalizationData.TrialTowerCoinBag.GetLocalizedString(), Color.yellow);
                    spawnCount = 15;
                } break;
            }
            
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.TrialTowerCapture);
            
            for (int i = 0; i < spawnCount; i++)
            {
                var enemyPrefab = _enemySpawnDirector.GetEnemiesNWavesAhead(2)[0];

                EnemyBase enemy = new EnemyBase();
                
                switch (tower.TrialTowerType)
                {
                    case TrialTowerType.Gambling:
                        enemy = enemyFactory.SpawnGamblingEnemy(enemyPrefab); break;
                    case TrialTowerType.GoldRush:
                        enemy = enemyFactory.SpawnGoldRushEnemy(enemyPrefab); break;
                }
                
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
            switch (tower.TrialTowerType)
            {
                case TrialTowerType.Gambling:
                    _itemFactory.SpawnGamblingItem(lastEnemyPosition); break;
            }
        }
    }
}