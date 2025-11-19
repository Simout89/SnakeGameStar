using System;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Combat;
using Users.FateX.Scripts.Utils;

namespace Users.FateX.Scripts.Upgrade
{
    public class MortarSnakeSegment: CombatSnakeSegment
    {
        [Header("References")]
        [SerializeField] private Transform gunPivot;
        
        
        public override void Attack()
        {
            base.Attack();

            EnemyBase[] enemyBases = EnemyFinder.GetRandomEnemies(transform.position, CurrentStats.AttackRange, CurrentStats.ProjectileCount);
            
            foreach (var enemy in enemyBases)
            {
                var projectile = LeanPool.Spawn(upgradeLevelsData.Projectile, gunPivot.position, Quaternion.identity);
                
                projectile.Launch(enemy.transform, 1f, CurrentStats.Damage, CurrentStats.DamageArea);
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