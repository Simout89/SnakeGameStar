using UnityEngine;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Upgrade;

namespace Users.FateX.Scripts.Achievements
{
    [CreateAssetMenu(menuName = "Data/AchievementData")]
    public class AchievementData: ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public AchievementType AchievementType { get; private set; }
        [field: SerializeField] public int RequiredValue { get; private set; }
        [field: SerializeField] public SnakeSegmentBase SnakeSegmentBase { get; private set; }
        [field: SerializeField] public CardData CardData { get; private set; }
    }

    public enum AchievementType
    {
        Kill,
        KillWithWeapon,
        LevelUp,
        LiveTime
    }
}