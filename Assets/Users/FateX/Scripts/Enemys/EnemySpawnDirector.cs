using System;
using System.Collections.Generic;
using UnityEngine;
using Users.FateX.Scripts.Data.WaveData;
using Zenject;

namespace Users.FateX.Scripts.Enemy
{
    public class EnemySpawnDirector : IInitializable, IDisposable
    {
        [Inject] private GameTimer _gameTimer;
        [Inject] private EnemyFactory _enemyFactory;

        private float _enemySpawnBuffer = 0f;
        private WaveData _waveData;

        // ————— Новое —————
        private HashSet<WaveEventSpawn> _triggeredEvents = new();
        private float _overrideSpawnRate = -1;
        private float _multiplySpawnRate = -1;

        private int _currentEnemyArrayIndex = 0;
        // ——————————————

        public event Action<float> OnChangeSpawnEnemyCount;

        public void SetWaveData(WaveData waveData)
        {
            _waveData = waveData;
        }

        public void Initialize()
        {
            _gameTimer.OnSecondChanged += HandleSecondTick;
        }

        public void Dispose()
        {
            _gameTimer.OnSecondChanged -= HandleSecondTick;
        }

        private void HandleSecondTick(int obj)
        {
            if (_waveData == null) return;

            float normalizedTime = (float)obj / _waveData.TotalTime;

            // ——— ИВЕНТЫ ———
            HandleEventSpawns(normalizedTime);
            // ————————————

            int currentEnemyIndex = 0;
            float segmentProgress = 0f;

            for (int i = 0; i < _waveData.WaveChangeSpawns.Length; i++)
            {
                if (normalizedTime >= _waveData.WaveChangeSpawns[i].TimeMarker)
                {
                    currentEnemyIndex = i;

                    float currentMarker = _waveData.WaveChangeSpawns[i].TimeMarker;
                    float nextMarker = (i + 1 < _waveData.WaveChangeSpawns.Length)
                        ? _waveData.WaveChangeSpawns[i + 1].TimeMarker
                        : 1f;

                    float segmentDuration = nextMarker - currentMarker;
                    segmentProgress = (normalizedTime - currentMarker) / segmentDuration;
                }
                else
                    break;
            }

            float enemiesToAdd = GetEnemyCountFloat(segmentProgress, currentEnemyIndex);

            // применяем Override / Multiply
            enemiesToAdd = ApplyEventModifiers(enemiesToAdd);

            _enemySpawnBuffer += enemiesToAdd;

            int spawnCount = Mathf.FloorToInt(_enemySpawnBuffer);

            if (spawnCount > 0)
            {
                for (int i = 0; i < spawnCount; i++)
                    SpawnNextEnemyInArray(_waveData.WaveChangeSpawns[currentEnemyIndex].Enemy);

                _enemySpawnBuffer -= spawnCount;
            }
        }

        // ——————————————————————————
        //           EVENT LOGIC
        // ——————————————————————————
        private void HandleEventSpawns(float normalizedTime)
        {
            foreach (var evt in _waveData.WaveEventSpawns)
            {
                if (_triggeredEvents.Contains(evt))
                    continue;

                if (normalizedTime >= evt.TimeMarker)
                {
                    // Спавним всех врагов сразу
                    foreach (var enemy in evt.Enemys)
                        _enemyFactory.SpawnEnemy(enemy);

                    // Устанавливаем модификаторы
                    _overrideSpawnRate = evt.OverrideSpawnRate;
                    _multiplySpawnRate = evt.MultiplaySpawnRate;

                    _triggeredEvents.Add(evt);
                }
            }
        }

        private float ApplyEventModifiers(float value)
        {
            if (_overrideSpawnRate != -1)
                value = _overrideSpawnRate;

            if (_multiplySpawnRate != -1)
                value *= _multiplySpawnRate;

            return value;
        }

        // ——————————————————————————
        //    MAIN SPAWN CURVE LOGIC
        // ——————————————————————————
        public float GetEnemyCountFloat(float segmentProgress, int enemyIndex)
        {
            float baseLevel = 1f + enemyIndex * 0.1f;
            float timeGrowth = 1f + _gameTimer.CurrentTime * 0.002f;

            float sawtoothWave = segmentProgress;
            float dropoffThreshold = 0.9f;
            if (sawtoothWave > dropoffThreshold)
            {
                float dropProgress = (sawtoothWave - dropoffThreshold) / (1f - dropoffThreshold);
                sawtoothWave = Mathf.Lerp(1f, 0.13f, dropProgress * dropProgress);
            }

            float intensityMultiplier = 12f + 10f * sawtoothWave;
            float spawnIntensity = (baseLevel * timeGrowth) * intensityMultiplier;
            spawnIntensity = Mathf.Clamp(spawnIntensity, 0.5f, 99999f);

            float enemiesPerSecond = spawnIntensity / 15f;
            OnChangeSpawnEnemyCount?.Invoke(enemiesPerSecond);

            return enemiesPerSecond;
        }

        // ——————————————————————————
        //       SPAWN ENEMIES SEQUENTIALLY
        // ——————————————————————————
        private void SpawnNextEnemyInArray(EnemyBase[] enemies)
        {
            if (enemies == null || enemies.Length == 0)
                return;

            _enemyFactory.SpawnEnemy(enemies[_currentEnemyArrayIndex]);
            _currentEnemyArrayIndex = (_currentEnemyArrayIndex + 1) % enemies.Length;
        }
    }
}
