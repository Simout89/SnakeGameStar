using System;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts.Achievements
{
    public class AchievementManager: IInitializable, IDisposable
    {
        public void Initialize()
        {
            GameEvents.OnDealDamage += HandleDealDamage;
        }

        public void Dispose()
        {
            GameEvents.OnDealDamage -= HandleDealDamage;
        }

        private void HandleDealDamage(DamageInfo obj)
        {
            Debug.Log("Нанесен урон");
        }
    }
}