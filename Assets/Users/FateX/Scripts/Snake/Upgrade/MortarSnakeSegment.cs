using System;
using UnityEngine;
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