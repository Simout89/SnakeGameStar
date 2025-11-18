using System;
using UnityEngine;
using Users.FateX.Scripts.Combat;

namespace Users.FateX.Scripts.Data.Upgrade
{
    [CreateAssetMenu(menuName = "Data/UpgradeLevelsData")]
    public class UpgradeLevelsData: ScriptableObject
    {
        [field: SerializeField] public UpgradeStats[] UpgradeStats { get; private set; }
        [field: SerializeField] public Projectile Projectile { get; private set; }
    }

    [Serializable]
    public class UpgradeStats
    {
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float DelayBetweenShots { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float ProjectileCount { get; private set; }
        [field: SerializeField] public float DamageArea { get; private set; }
        [field: SerializeField] public float BouncesCount { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
    }
}