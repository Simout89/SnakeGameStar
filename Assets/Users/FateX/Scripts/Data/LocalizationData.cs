using UnityEngine;
using UnityEngine.Localization;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/LocalizationData")]
    public class LocalizationData: ScriptableObject
    {
        [field: SerializeField] public LocalizedString TrialTowerSpawnText { get; private set; }
        [field: SerializeField] public LocalizedString TrialTowerGambler { get; private set; }
        [field: SerializeField] public LocalizedString TrialTowerCoinBag { get; private set; }
        
        
        [field: SerializeField] public LocalizedString TimeSurvived { get; private set; }
        [field: SerializeField] public LocalizedString CoinsEarned { get; private set; }
        [field: SerializeField] public LocalizedString EnemiesKilled { get; private set; }
        [field: SerializeField] public LocalizedString SegmentsGained { get; private set; }
        [field: SerializeField] public LocalizedString ApplesEaten { get; private set; }
        [field: SerializeField] public LocalizedString MagnetsUsed { get; private set; }
        [field: SerializeField] public LocalizedString LevelsGained { get; private set; }
        
        [field: SerializeField] public LocalizedString Gained  { get; private set; }
    }
}