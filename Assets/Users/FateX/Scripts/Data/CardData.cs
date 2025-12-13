using UnityEngine;
using UnityEngine.Localization;
using Users.FateX.Scripts.Upgrade;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/CardData")]
    public class CardData: ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public bool IsObtained { get; private set; } = false;
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public CardType CardType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public SnakeSegmentBase SnakeSegmentBase { get; private set; }
        [field: TextArea(10, 10)]
        [field: SerializeField] 
        public string Description { get; private set; }
        [field: SerializeField] public LocalizedString LocalizedDescription { get; private set; }

    }

    public enum CardType
    {
        Segment,
        Upgrade,
        Coin,
        Heal
    }
}