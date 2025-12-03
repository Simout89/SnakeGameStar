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