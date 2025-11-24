using System;
using UnityEngine;

namespace Users.FateX.Scripts.Combat
{
    public class DamageOrb: MonoBehaviour
    {
        private DamageInfo damageInfo;
        
        public void Init(DamageInfo damageInfo)
        {
            this.damageInfo = damageInfo;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damageInfo);
                
                GameEvents.DamageDealt(damageInfo);
            }
        }
    }
}