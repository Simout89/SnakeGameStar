using System;
using UnityEngine;
using Users.FateX.Scripts.Combat;

namespace Users.FateX.Scripts.Data.Upgrade
{
    [CreateAssetMenu(menuName = "Data/UpgradeLevelsData")]
    public class UpgradeLevelsData: ScriptableObject
    {
        [Header("Settings")]
        [field: SerializeField] public string SegmentName { get; private set; }
        [field: SerializeField] public Sprite SegmentIcon { get; private set; }
        [field: SerializeField] public int BaseSegmentsCount { get; private set; } = 1;
        [field: SerializeField] public Projectile Projectile { get; private set; }
        [Header("Upgrade")]
        [field: SerializeField] public UpgradeStats[] UpgradeStats { get; private set; }
        [Header("Visual")]
        [field: SerializeField] public GameObject Vfx { get; private set; }
    }

    [Serializable]
    public class UpgradeStats
    {
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float DelayBetweenShots { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public int ProjectileCount { get; private set; }
        [field: SerializeField] public float DamageArea { get; private set; }
        [field: SerializeField] public int BouncesCount { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
    }
}