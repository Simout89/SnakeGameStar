using System;
using UnityEngine;
using Users.FateX.Scripts.Data;
using Zenject;

namespace Users.FateX.Scripts.Achievements
{
    public class AchievementManager: IInitializable, IDisposable
    {
        [Inject] private GlobalSoundPlayer _globalSoundPlayer;
        public void Initialize()
        {
            GameEvents.OnDealDamage += HandleDealDamage;
            GameEvents.OnEnemyDie += HandleEnemyDie;
;        }

        public void Dispose()
        {
            GameEvents.OnDealDamage -= HandleDealDamage;
            GameEvents.OnEnemyDie -= HandleEnemyDie;

        }

        private void HandleEnemyDie(EnemyData obj, DamageInfo damageInfo)
        {
            Debug.Log($"Умер {obj.EnemyName} от {damageInfo.DamageDealerName}");
        }

        private void HandleDealDamage(DamageInfo obj)
        {
            Debug.Log($"Нанесен урон {obj.DamageDealerName} кол-во: {obj.Amount}");
            _globalSoundPlayer.Play(_globalSoundPlayer.SoundsData.DamageSound);
        }
    }
}