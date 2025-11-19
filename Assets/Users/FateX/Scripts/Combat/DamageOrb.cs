using System;
using UnityEngine;

namespace Users.FateX.Scripts.Combat
{
    public class DamageOrb: MonoBehaviour
    {
        private float damage;
        
        public void Init(float damage)
        {
            this.damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(new DamageInfo(damage));
            }
        }
    }
}