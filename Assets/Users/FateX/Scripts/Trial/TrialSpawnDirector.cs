using System;
using System.Collections.Generic;
using UnityEngine;
using Users.FateX.Scripts.Utils;
using Users.FateX.Scripts.View;
using Zenject;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.Trial
{
    public class TrialSpawnDirector : IInitializable, IDisposable
    {
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        [Inject] private GameConfig _gameConfig;
        [Inject] private TrialTowerFactory _trialTowerFactory;
        [Inject] private GameTimer _gameTimer;
        [Inject] private MessageDisplayView _messageDisplayView;
        public List<GameObject> availablePoints = new List<GameObject>();
        private WeightedRandomGenerator _weightedRandomGenerator;

        public void Initialize()
        {
            _gameTimer.OnSecondChanged += HandleSecond;

            var spawnPoints = GameObject.FindGameObjectsWithTag("TrialTowerPoint");
            availablePoints = new List<GameObject>(spawnPoints);

            _weightedRandomGenerator = new WeightedRandomGenerator(0, Enum.GetValues(typeof(TrialTowerType)).Length - 1, 1.5f);
        }

        public void SpawnRandomTower()
        {
            if (availablePoints.Count > 0)
            {
                var randomPoint = availablePoints[_weightedRandomGenerator.Next()];
                availablePoints.Remove(randomPoint);

                _trialTowerFactory.SpawnTowerByType(
                    (TrialTowerType)Random.Range(0, Enum.GetValues(typeof(TrialTowerType)).Length),
                    randomPoint.transform.position);

                _messageDisplayView.ShowText("Появилась башня испытаний", Color.cyan);
                _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.DisplayMessage);
            }
            else
            {
            }
        }

        public void Dispose()
        {
            _gameTimer.OnSecondChanged -= HandleSecond;
        }

        private void HandleSecond(int obj)
        {
            if (obj % _gameConfig.GameConfigData.SpawnTrialTowerEverySeconds == 0)
            {
                SpawnRandomTower();
            }
        }
    }
}