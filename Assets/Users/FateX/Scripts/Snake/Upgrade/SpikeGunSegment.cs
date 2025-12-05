using Lean.Pool;
using UnityEngine;

namespace Users.FateX.Scripts.Upgrade
{
    public class SpikeGunSegment: CombatSnakeSegment
    {
        [SerializeField] private float maxSpreadAngle = 45f; // Максимальный угол разброса на каждую сторону
        
        public override void Attack()
        {
            base.Attack();
            
            int totalProjectiles = CurrentStats.ProjectileCount + SnakeController.PlayerStats.ProjectileCount.Sum;
            
            for (int i = 0; i < totalProjectiles; i++)
            {
                // Определяем сторону: чётные индексы - влево, нечётные - вправо
                bool isLeft = (i % 2 == 0);
                float baseSideAngle = isLeft ? -90f : 90f;
                
                float spreadAngle = 0f;
                if (totalProjectiles > 2)
                {
                    int sideIndex = i / 2; // Номер снаряда на этой стороне
                    int projectilesPerSide = (totalProjectiles + 1) / 2; // Снарядов на одну сторону
                    
                    if (projectilesPerSide > 1)
                    {
                        // Распределяем снаряды в фиксированном диапазоне
                        // Чем больше снарядов, тем плотнее они расположены
                        spreadAngle = Mathf.Lerp(-maxSpreadAngle, maxSpreadAngle, 
                            sideIndex / (float)(projectilesPerSide - 1));
                    }
                }
                
                float finalAngle = baseSideAngle + spreadAngle;
                var rotation = Quaternion.Euler(0, 0, Body.transform.localEulerAngles.z + finalAngle);
                
                var projectile = LeanPool.Spawn(
                    UpgradeLevelsData.Projectile,
                    Body.position,
                    rotation
                );

                projectile.SimpleLaunch(
                    rotation * Vector2.up * 15,
                    new DamageInfo(CurrentStats.Damage + SnakeController.PlayerStats.Damage.Sum,
                        UpgradeLevelsData.SegmentName, Body.transform.position), true
                );
            }
        }
    }
}