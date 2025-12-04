using System;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Combat;
using Users.FateX.Scripts.Utils;

namespace Users.FateX.Scripts.Upgrade
{
    public class MortarSnakeSegment : CombatSnakeSegment
    {
        [Header("References")] [SerializeField]
        private Transform gunPivot;


        public override void Attack()
        {
            base.Attack();

            EnemyBase[] enemyBases = EnemyFinder.GetRandomEnemies(gunPivot.position, CurrentStats.AttackRange,
                CurrentStats.ProjectileCount + SnakeController.PlayerStats.ProjectileCount.Sum);

            foreach (var enemy in enemyBases)
            {
                var projectile = LeanPool.Spawn(upgradeLevelsData.Projectile, gunPivot.position, Quaternion.identity);

                projectile.LaunchArc(enemy.transform, 1f,
                    new DamageInfo(CurrentStats.Damage + SnakeController.PlayerStats.Damage.Sum,
                        upgradeLevelsData.SegmentName, Body.transform.position), CurrentStats.DamageArea);
            }
        }

        private void OnDrawGizmos()
        {
            if (upgradeLevelsData == null || gunPivot.position == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gunPivot.position, upgradeLevelsData.UpgradeStats[0].AttackRange);
        }
    }
}