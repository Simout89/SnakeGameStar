using System;

namespace Users.FateX.Scripts
{
    public static class GameEvents
    {
        public static event Action<DamageInfo> OnDealDamage;
        
        public static void DamageDealt(DamageInfo damageInfo)
        {
            OnDealDamage?.Invoke(damageInfo);
        }
    }
}