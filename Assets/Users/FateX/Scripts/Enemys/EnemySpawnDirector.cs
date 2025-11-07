using System;
using Users.FateX.Scripts.Data.WaveData;
using Zenject;

namespace Users.FateX.Scripts.Enemy
{
    public class EnemySpawnDirector: IInitializable, IDisposable
    {
        [Inject] private GameTimer _gameTimer;
        [Inject] private EnemySpawner _enemySpawner;

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
    
            for (int i = 0; i < _waveData.WaveChangeSpawns.Length; i++)
            {
                if (normalizedTime >= _waveData.WaveChangeSpawns[i].TimeMarker)
                {
                    currentEnemyIndex = i;
                }
                else
                {
                    break;
                }
            }
    
            _enemySpawner.SpawnEnemy(_waveData.WaveChangeSpawns[currentEnemyIndex].Enemy);
        }
    }
}