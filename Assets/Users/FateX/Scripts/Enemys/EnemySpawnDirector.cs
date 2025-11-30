using System;
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

        public event Action<float> OnChangeSpawnEnemyCount;

        private WaveData _waveData;

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
            float normalizedTime = (float)obj / _waveData.TotalTime;

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

            _enemySpawnBuffer += enemiesToAdd;

            int spawnCount = Mathf.FloorToInt(_enemySpawnBuffer);

            if (spawnCount > 0)
            {
                for (int i = 0; i < spawnCount; i++)
                    _enemyFactory.SpawnEnemy(_waveData.WaveChangeSpawns[currentEnemyIndex].Enemy);

                _enemySpawnBuffer -= spawnCount;
            }
        }


        public float GetEnemyCountFloat(float segmentProgress, int enemyIndex)
        {
            // Базовый рост зависит от индекса врага
            float baseLevel = 1f + enemyIndex * 0.1f;
    
            // Дополнительный рост по времени
            float timeGrowth = 1f + _gameTimer.CurrentTime * 0.002f;
    
            // Пилообразная волна
            float sawtoothWave = segmentProgress;
    
            // Резкое падение в конце сегмента
            float dropoffThreshold = 0.9f;
            if (sawtoothWave > dropoffThreshold)
            {
                float dropProgress = (sawtoothWave - dropoffThreshold) / (1f - dropoffThreshold);
                sawtoothWave = Mathf.Lerp(1f, 0.13f, dropProgress * dropProgress);
            }
    
            // Комбинируем
            float intensityMultiplier = 12f + 10f * sawtoothWave;
            float spawnIntensity = (baseLevel * timeGrowth) * intensityMultiplier;
    
            // Ограничиваем
            spawnIntensity = Mathf.Clamp(spawnIntensity, 0.5f, 99999f);
    
            // Конвертируем в врагов в секунду
            float enemiesPerSecond = spawnIntensity / 15f;

            OnChangeSpawnEnemyCount?.Invoke(enemiesPerSecond);

            return enemiesPerSecond;
        }
        
    }
}