using Lean.Pool;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts.Trial
{
    public class TrialTowerFactory
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private DiContainer _diContainer;

        public void SpawnTowerByType(TrialTowerType trialTowerType, Vector3 position)
        {
            switch (trialTowerType)
            {
                case TrialTowerType.Gambling: SpawnGamblingTower(position);
                    break;
                case TrialTowerType.GoldRush: SpawnGoldRushTower(position);
                    break;
            }
        }

        public void SpawnGamblingTower(Vector3 positions)
        {
            var newTower = LeanPool.Spawn(_gameConfig.GameConfigData.TrialTower);
            _diContainer.InjectGameObject(newTower.gameObject);
            newTower.TowerSprite.material = _gameConfig.GameConfigData.EnemyMaterials.RainbowMaterial;
            newTower.TrialTowerType = TrialTowerType.Gambling;
            newTower.transform.position = positions;
        }
        
        public void SpawnGoldRushTower(Vector3 positions)
        {
            var newTower = LeanPool.Spawn(_gameConfig.GameConfigData.TrialTower);
            _diContainer.InjectGameObject(newTower.gameObject);
            newTower.TowerSprite.material = _gameConfig.GameConfigData.EnemyMaterials.GoldMaterial;
            newTower.TrialTowerType = TrialTowerType.GoldRush;
            newTower.transform.position = positions;
        }
    }

    public enum TrialTowerType
    {
        Gambling,
        GoldRush
    }
}