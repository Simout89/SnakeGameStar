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

        private HashSet<WaveEventSpawn> _triggeredEvents = new();
        private float _overrideSpawnRate = -1;
        private float _multiplySpawnRate = -1;

        private int _currentEnemyArrayIndex = 0;
        private int _currentWaveIndex = 0;
        private bool _allWavesCompleted = false;
        private float _infinityEnemyIndex = 0;
        private float _infinityStatMultiplier = 1f;


        public event Action<float> OnChangeSpawnEnemyCount;
        public event Action OnAllWavesCompleted;

        public void SetWaveData(WaveData waveData)
        {
            _waveData = waveData;
            _allWavesCompleted = false;
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

            if (_allWavesCompleted)
            {
                InfinityMobSpawn(obj);
                return;
            }

            if (normalizedTime >= 1f && !_allWavesCompleted)
            {
                _allWavesCompleted = true;
                OnAllWavesCompleted?.Invoke();
                return;
            }

            HandleEventSpawns(normalizedTime);

            int currentEnemyIndex = 0;
            float segmentProgress = 0f;

            for (int i = 0; i < _waveData.WaveChangeSpawns.Length; i++)
            {
                if (normalizedTime >= _waveData.WaveChangeSpawns[i].TimeMarker)
                {
                    currentEnemyIndex = i;
                    _currentWaveIndex = i;

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

        private void InfinityMobSpawn(int currentTime)
        {
            // Индекс врагов растёт — влияет на количество
            _infinityEnemyIndex += Time.deltaTime * 0.2f;
            if (_infinityEnemyIndex > 9999f)
                _infinityEnemyIndex = 9999f;

            // Множитель статов растёт непрерывно
            _infinityStatMultiplier += Time.deltaTime * 0.03f;

            // Максимальный прогресс сегмента
            float segmentProgress = 1f;

            // Формула спавна — твоя же
            float enemiesToAdd = GetEnemyCountFloat(segmentProgress, Mathf.FloorToInt(_infinityEnemyIndex));
            enemiesToAdd = ApplyEventModifiers(enemiesToAdd);
            _enemySpawnBuffer += enemiesToAdd;

            int spawnCount = Mathf.FloorToInt(_enemySpawnBuffer);

            if (spawnCount > 0)
            {
                var finalEnemies = _waveData.WaveChangeSpawns[^1].Enemy;

                for (int i = 0; i < spawnCount; i++)
                {
                    _enemyFactory.SpawnFinalEnemy(
                        _infinityStatMultiplier
                    );

                    _currentEnemyArrayIndex = (_currentEnemyArrayIndex + 1) % finalEnemies.Length;
                }

                _enemySpawnBuffer -= spawnCount;
            }
        }

        // ——————————————————————————
        //    МЕТОДЫ ПОЛУЧЕНИЯ ВРАГОВ
        // ——————————————————————————

        public EnemyBase[] GetCurrentWaveEnemies()
        {
            if (_waveData == null || _waveData.WaveChangeSpawns == null || _waveData.WaveChangeSpawns.Length == 0)
                return Array.Empty<EnemyBase>();

            if (_currentWaveIndex >= _waveData.WaveChangeSpawns.Length)
                return Array.Empty<EnemyBase>();

            return _waveData.WaveChangeSpawns[_currentWaveIndex].Enemy;
        }

        public EnemyBase[] GetEnemiesNWavesAhead(int wavesAhead)
        {
            if (_waveData == null || _waveData.WaveChangeSpawns == null || _waveData.WaveChangeSpawns.Length == 0)
                return Array.Empty<EnemyBase>();

            int targetWaveIndex = _currentWaveIndex + wavesAhead;

            if (targetWaveIndex < 0)
                targetWaveIndex = 0;

            if (targetWaveIndex >= _waveData.WaveChangeSpawns.Length)
                targetWaveIndex = _waveData.WaveChangeSpawns.Length - 1;

            return _waveData.WaveChangeSpawns[targetWaveIndex].Enemy;
        }

        public int GetCurrentWaveIndex()
        {
            return _currentWaveIndex;
        }

        public int GetTotalWavesCount()
        {
            if (_waveData == null || _waveData.WaveChangeSpawns == null)
                return 0;

            return _waveData.WaveChangeSpawns.Length;
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
                    foreach (var enemy in evt.Enemys)
                        _enemyFactory.SpawnEnemy(enemy);

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

            if (_currentEnemyArrayIndex >= enemies.Length)
                _currentEnemyArrayIndex = 0;

            _enemyFactory.SpawnEnemy(enemies[_currentEnemyArrayIndex]);
            _currentEnemyArrayIndex = (_currentEnemyArrayIndex + 1) % enemies.Length;
        }
    }
}