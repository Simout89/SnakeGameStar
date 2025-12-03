using UnityEngine;
using Users.FateX.Scripts.Upgrade;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/SlotMachinePrizeData")]
    public class SlotMachinePrizeData: ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public SlotMachinePrizeType SlotMachinePrizeType { get; private set; }
        [field: SerializeField] public SnakeSegmentBase[] SnakeSegmentBase { get; private set; }
        [field: SerializeField] public float Amount { get; private set; }
        
        [Header("Вероятность")]
        [field: SerializeField] 
        [field: Range(0.01f, 100f)]
        [field: Tooltip("Вес приза. Чем больше значение, тем выше шанс выпадения")]
        public float Weight { get; private set; } = 1f;
    }

    public enum SlotMachinePrizeType
    {
        Coin,
        Xp,
        Magnet,
        Heal,
        Segment
    }
}