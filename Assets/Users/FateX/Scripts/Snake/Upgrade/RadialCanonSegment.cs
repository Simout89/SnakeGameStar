using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.Upgrade
{
    public class RadialCanonSegment: CombatSnakeSegment
    {
        public override void Attack()
        {
            base.Attack();
            
            for (int i = 0; i < CurrentStats.ProjectileCount + SnakeController.PlayerStats.ProjectileCount.Sum; i++)
            {
                var direction = 360 / (CurrentStats.ProjectileCount + SnakeController.PlayerStats.ProjectileCount.Sum) * i;

                var rotation = Quaternion.Euler(0, 0, direction);
                var projectile = LeanPool.Spawn(
                    UpgradeLevelsData.Projectile,
                    Body.position,
                    rotation
                );

                projectile.SimpleLaunch(
                    rotation * Vector2.up * 15,
                    new DamageInfo(CurrentStats.Damage + SnakeController.PlayerStats.Damage.Sum,
                        UpgradeLevelsData.SegmentName, Body.transform.position)
                );
            }
        }

        public override void Upgrade()
        {
            base.Upgrade();
        }
    }
}