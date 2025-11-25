using System;
using TMPro;
using UnityEngine;
using Users.FateX.Scripts.Enemy;
using Zenject;

namespace Users.FateX.Scripts.DebugUtils
{
    public class DebugEnemyInfo: MonoBehaviour
    {
        [Inject] private EnemySpawnDirector _spawnDirector;
        [Inject] private EnemyManager _enemyManager;

        [SerializeField] private TMP_Text enemyCount;
        [SerializeField] private TMP_Text enemySpawnCount;

        private void OnEnable()
        {
            _enemyManager.AliveEnemyCountChanged += HandleEnemyCountChanged;
            _spawnDirector.OnChangeSpawnEnemyCount += HandleChangedSpawnEnemyCount;
        }

        private void OnDisable()
        {
            _enemyManager.AliveEnemyCountChanged -= HandleEnemyCountChanged;
            _spawnDirector.OnChangeSpawnEnemyCount -= HandleChangedSpawnEnemyCount;
        }

        private void HandleChangedSpawnEnemyCount(float obj)
        {
            enemySpawnCount.text = $"Врагов спавнится за раз: {obj.ToString("F5")}";
        }

        private void HandleEnemyCountChanged(int obj)
        {
            enemyCount.text = $"Живо врагов: {obj}";
        }
    }
}