using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Upgrade;
using Users.FateX.Scripts.Utils;

namespace Users.FateX.Scripts
{
    public class PentagramSegment: CombatSnakeSegment
    {
        public override void Attack()
        {
            base.Attack();
            
            EnemyBase[] enemyBases = EnemyFinder.GetRandomEnemies(Body.position, CurrentStats.AttackRange,
                CurrentStats.ProjectileCount + SnakeController.PlayerStats.ProjectileCount.Sum);

            foreach (var enemy in enemyBases)
            {
                var effect = LeanPool.Spawn(UpgradeLevelsData.Vfx, enemy.transform.position, Quaternion.identity);
                effect.transform.localScale = Vector3.zero;
                effect.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 180));
                var seq = DOTween.Sequence();
                seq.Append(effect.transform.DOScale(Vector3.one, 0.3f));
                seq.AppendInterval(0.1f);
                seq.Append(effect.transform.DOScale(Vector3.zero, 0.3f));
                seq.OnComplete(() => LeanPool.Despawn(effect));
                
                enemy.TakeDamage(new DamageInfo(CurrentStats.Damage + SnakeController.PlayerStats.Damage.Sum,
                    UpgradeLevelsData.SegmentName, Body.transform.position));

                if (Random.Range(0, 101) == 0)
                {
                    SnakeController.SnakeHealth.Heal(1f);
                }
            }
        }
    }
}