using System;
using DG.Tweening;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;
using Users.FateX.Scripts.Combat;
using Users.FateX.Scripts.Utils;

namespace Users.FateX.Scripts.Upgrade
{
    public class SpikedOrbSegment : CombatSnakeSegment
    {
        [Header("References")] [SerializeField]
        private Transform orbContainer;

        [SerializeField] private DamageOrb damageOrbPrefab;


        public override void Init(SnakeController snakeController)
        {
            base.Init(snakeController);

            UpdateOrbCount();
        }

        public override void Tick()
        {
            base.Tick();
            
            orbContainer.Rotate(0f, 0, 150f * Time.deltaTime);
        }

        public override void Upgrade()
        {
            base.Upgrade();

            UpdateOrbCount();
        }

        private void UpdateOrbCount()
        {
            foreach (Transform child in orbContainer)
            {
                Destroy(child.gameObject);
            }

            var damageInfo = new DamageInfo(CurrentStats.Damage + SnakeController.PlayerStats.Damage.Sum, upgradeLevelsData.SegmentName);
            
            foreach (var position in MyUtils.GetPositionsInCircle2D(orbContainer.position, CurrentStats.AttackRange,
                         CurrentStats.ProjectileCount + SnakeController.PlayerStats.ProjectileCount.Sum))
            {
                var newOrb = Instantiate(damageOrbPrefab, position, Quaternion.identity, orbContainer);
                newOrb.Init(damageInfo);
            }
        }

        private void OnDrawGizmos()
        {
            if (upgradeLevelsData == null || orbContainer.position == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(orbContainer.position, upgradeLevelsData.UpgradeStats[0].AttackRange);
        }
    }
}