using Lean.Pool;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts.Trial
{
    public class TrialTowerFactory
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private DiContainer _diContainer;

        public TrialTower SpawnTowerByType(TrialTowerType trialTowerType, Vector3 position)
        {
            switch (trialTowerType)
            {
                case TrialTowerType.Gambling: return SpawnGamblingTower(position);
                    break;
                case TrialTowerType.GoldRush: return SpawnGoldRushTower(position);
                    break;
            }

            return null;
        }

        public TrialTower SpawnGamblingTower(Vector3 positions)
        {
            var newTower = LeanPool.Spawn(_gameConfig.GameConfigData.TrialTower);
            _diContainer.InjectGameObject(newTower.gameObject);
            newTower.TowerSprite.material = _gameConfig.GameConfigData.EnemyMaterials.RainbowMaterial;
            newTower.TrialTowerType = TrialTowerType.Gambling;
            newTower.transform.position = positions;
            return newTower;
        }
        
        public TrialTower SpawnGoldRushTower(Vector3 positions)
        {
            var newTower = LeanPool.Spawn(_gameConfig.GameConfigData.TrialTower);
            _diContainer.InjectGameObject(newTower.gameObject);
            newTower.TowerSprite.material = _gameConfig.GameConfigData.EnemyMaterials.GoldMaterial;
            newTower.TrialTowerType = TrialTowerType.GoldRush;
            newTower.transform.position = positions;
            return newTower;

        }
    }

    public enum TrialTowerType
    {
        Gambling,
        GoldRush
    }
}