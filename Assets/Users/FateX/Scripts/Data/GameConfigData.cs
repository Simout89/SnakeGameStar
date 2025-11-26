using UnityEngine;
using Users.FateX.Scripts.Cards;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.View;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/GameConfigData")]
    public class GameConfigData: ScriptableObject
    {
        [field: SerializeField] public XpItem XpPrefab { get; private set; }
        [field: SerializeField] public MagnetItem MagnetPrefab { get; private set; }
        [field: SerializeField] public CardEntryView CardPrefab { get; private set; }
        [field: SerializeField] public DamageView DamageViewPrefab { get; private set; }
        [field: SerializeField] public CardData[] CardDatas { get; private set; }

    }
}