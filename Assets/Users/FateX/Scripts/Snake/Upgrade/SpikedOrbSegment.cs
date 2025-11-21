using System;
using DG.Tweening;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;
using Users.FateX.Scripts.Combat;
using Users.FateX.Scripts.Utils;

namespace Users.FateX.Scripts.Upgrade
{
    public class SpikedOrbSegment: CombatSnakeSegment
    {
        [Header("References")]
        [SerializeField] private Transform orbContainer;
        [SerializeField] private DamageOrb damageOrbPrefab;


        public override void Init(SnakeController snakeController)
        {
            base.Init(snakeController);

            StartRotate();

            UpdateOrbCount();
        }

        private void StartRotate()
        {
            orbContainer.DOKill();
            
            orbContainer.DOLocalRotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        public override void Upgrade()
        {
            base.Upgrade();
            
            UpdateOrbCount();
        }

        private void UpdateOrbCount()
        {
            orbContainer.DOKill();
            
            foreach (Transform child in orbContainer)
            {
                child.DOKill(true);
                Destroy(child.gameObject);
            }

            foreach (var position in MyUtils.GetPositionsInCircle2D(orbContainer.position, CurrentStats.AttackRange, CurrentStats.ProjectileCount))
            {
                var newOrb = Instantiate(damageOrbPrefab, position, Quaternion.identity, orbContainer);
                newOrb.Init(CurrentStats.Damage);
            }

            StartRotate();
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