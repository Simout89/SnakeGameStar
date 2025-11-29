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
        [field: SerializeField] public float Cost { get; private set; }
        [field: SerializeField] public float GoldDropChance { get; private set; }
    }
}