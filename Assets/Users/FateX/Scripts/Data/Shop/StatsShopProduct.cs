using System;
using UnityEngine;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/Shop/StatsShopProduct")]
    public class StatsShopProduct : ShopProductData
    {
        [field: SerializeField] public StatsUpgradeLevel[] StatsUpgradeLevels { get; private set; }

        public StatsUpgradeLevel GetStatsUpgradeByLevel(int level)
        {
            return StatsUpgradeLevels[level];
        }
    }

    [Serializable]
    public class StatsUpgradeLevel
    {
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public float GoldDropChance { get; private set; }
        [field: SerializeField] public int Exile { get; private set; }
        [field: SerializeField] public int Reroll { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public int ProjectileCount { get; private set; }
        [field: SerializeField] public int PickUpRange { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
    }
}