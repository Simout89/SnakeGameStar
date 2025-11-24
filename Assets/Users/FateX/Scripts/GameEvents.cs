using System;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts
{
    public static class GameEvents
    {
        public static event Action<DamageInfo> OnDealDamage;
        public static event Action<EnemyData, DamageInfo> OnEnemyDie; 
        
        public static void DamageDealt(DamageInfo damageInfo)
        {
            OnDealDamage?.Invoke(damageInfo);
        }
        
        public static void EnemyDie(EnemyData enemyData, DamageInfo lastDamageInfo)
        {
            OnEnemyDie?.Invoke(enemyData, lastDamageInfo);
        }
    }
}