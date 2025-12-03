using System;
using UnityEngine;
using Users.FateX.Scripts.Cards;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.View;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/GameConfigData")]
    public class GameConfigData: ScriptableObject
    {
        [field: SerializeField, Header("Prefabs")] public XpItem XpPrefab { get; private set; }
        [field: SerializeField] public MagnetItem MagnetPrefab { get; private set; }
        [field: SerializeField] public GamblingItem GamblingItemPrefab { get; private set; }
        [field: SerializeField] public CoinItem CoinItemPrefab { get; private set; }
        [field: SerializeField] public HealableItem HealableItemPrefab { get; private set; }
        [field: SerializeField] public CardEntryView CardPrefab { get; private set; }
        [field: SerializeField] public DamageView DamageViewPrefab { get; private set; }
        [field: SerializeField] public SpecialCards SpecialCards { get; private set; }
        [field: SerializeField] public CardData[] CardDatas { get; private set; }
        [field: SerializeField] public StatsShopProduct[] StatsShopProducts { get; private set; }
        [field: SerializeField] public SlotMachinePrizeData[] SlotMachinePrizeDatas { get; private set; }
        [field: SerializeField, Header("Settings")] public float DropCoinChance { get; private set; }
        [field: SerializeField] public float XpValue { get; private set; } = 0.4f;
        [field: SerializeField] public float MagnetDropChance { get; private set; } = 0.5f;

    }

    [Serializable]
    public class SpecialCards
    {
        [field: SerializeField] public CardData UpgradeCard;
        [field: SerializeField] public CardData HealCard;
        [field: SerializeField] public CardData CoinCard;
    }
}