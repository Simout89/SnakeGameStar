using UnityEngine;
using Users.FateX.Scripts.Upgrade;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/CardData")]
    public class CardData: ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public CardType CardType { get; private set; }
        [field: SerializeField] public SnakeSegmentBase SnakeSegmentBase { get; private set; }
    }

    public enum CardType
    {
        Segment,
        Upgrade
    }
}