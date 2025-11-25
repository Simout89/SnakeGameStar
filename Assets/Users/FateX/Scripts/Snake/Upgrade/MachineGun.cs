using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Utils;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.Upgrade
{
    public class MachineGun : CombatSnakeSegment
    {
        [SerializeField] private Transform machineGun;
        [SerializeField] private Transform muzzle;

        private Transform nearestEnemy;

        private void Awake()
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, Random.Range(-360, 360));

            machineGun.localRotation = targetRotation;
        }

        public override void Tick()
        {
            base.Tick();

            nearestEnemy = EnemyFinder.GetNearestEnemy(machineGun.position, CurrentStats.AttackRange)?.transform;

            if (nearestEnemy != null)
            {
                Vector3 dir = nearestEnemy.transform.position - machineGun.position;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);

                machineGun.localRotation = Quaternion.Lerp(
                    machineGun.localRotation,
                    targetRotation,
                    Time.fixedDeltaTime * 5f
                );
            }
        }

        public override void Attack()
        {
            base.Attack();

            Shoot().Forget();
        }

        private async UniTask Shoot()
        {
            for (int i = 0; i < CurrentStats.ProjectileCount; i++)
            {
                var projectile = LeanPool.Spawn(
                    UpgradeLevelsData.Projectile,
                    muzzle.position,
                    muzzle.rotation
                );

                projectile.SimpleLaunch(
                    muzzle.up * 15,
                    new DamageInfo(CurrentStats.Damage, UpgradeLevelsData.SegmentName)
                );
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }
    }
}