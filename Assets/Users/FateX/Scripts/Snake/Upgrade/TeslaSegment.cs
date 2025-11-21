using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Users.FateX.Scripts.Data.Upgrade;
using Users.FateX.Scripts.Utils;

namespace Users.FateX.Scripts.Upgrade
{
    public class TeslaSegment : CombatSnakeSegment
    {
        private CancellationTokenSource _cancellationTokenSource;
        private GameObject lightingTrail;

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public override void Attack()
        {
            base.Attack();

            var enemy = EnemyFinder.GetChainEnemies(Body.position, CurrentStats.AttackRange, CurrentStats.BouncesCount);
            if (enemy.Length == 0) return;

            lightingTrail = LeanPool.Spawn(upgradeLevelsData.Vfx, Body.position, Quaternion.identity);

            // Не отменяем предыдущий токен
            var token = _cancellationTokenSource.Token;

            AlternateAttack(enemy, token).Forget();
        }


        private async UniTask AlternateAttack(EnemyBase[] enemyBase, CancellationToken cancellationToken)
        {
            foreach (var enemy in enemyBase)
            {
                if(Vector3.Distance(enemy.transform.position, lightingTrail.transform.position) > CurrentStats.AttackRange)
                    return;
                
                await lightingTrail.transform.DOMove(enemy.transform.position, 0.1f).AsyncWaitForCompletion();
                
                enemy.TakeDamage(new DamageInfo(CurrentStats.Damage));

                await UniTask.WaitForSeconds(0.1f, false, PlayerLoopTiming.Update, cancellationToken);
            }

            LeanPool.Despawn(lightingTrail, 1f);
        }
    }
}