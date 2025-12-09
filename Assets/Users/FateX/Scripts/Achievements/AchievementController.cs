using System;
using System.Linq;
using UnityEngine;
using Users.FateX.Scripts.Data;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.Achievements
{
    public class AchievementController : IInitializable, IDisposable
    {
        [Inject] private ISaveLoadService _saveLoadService;
        [InjectOptional] private IAchievementView _achievementView;
        
        private AchievementModel _achievementModel;

        public void Initialize()
        {
            _achievementModel = new AchievementModel(_saveLoadService.GetAchievements());
            
            GameEvents.OnDealDamage += HandleDealDamage;
            GameEvents.OnEnemyDie += HandleEnemyDie;

            if (_achievementView != null)
            {
                _achievementView.OnUpdate += HandleUpdateView;
            }
        }

        private void HandleEnemyDie(EnemyData arg1, DamageInfo arg2)
        {
            foreach (var achievement in _achievementModel.AchievementEntries)
            {
                if (achievement.Value.AchievementData.AchievementType == AchievementType.Kill)
                {
                    if(achievement.Value.AchievementSaveData.IsCompleted)
                        continue;
                    
                    achievement.Value.AchievementSaveData.Progress++;

                    if (achievement.Value.AchievementSaveData.Progress >=
                        achievement.Value.AchievementData.RequiredValue)
                    {
                        achievement.Value.AchievementSaveData.IsCompleted = true;
                        Debug.Log("Достижение выполнено");
                    }
                }
            }
        }

        private void HandleDealDamage(DamageInfo obj)
        {
            
        }

        public void Dispose()
        {
            GameEvents.OnDealDamage -= HandleDealDamage;
            GameEvents.OnEnemyDie -= HandleEnemyDie;
            
            if (_achievementView != null)
            {
                _achievementView.OnUpdate -= HandleUpdateView;
            }
            
            _saveLoadService.SaveAchievements(_achievementModel.AchievementEntries);

        }

        private void HandleUpdateView()
        {
            AchievementEntry[] achievementEntries = _achievementModel.AchievementEntries.Values.ToArray();

            _achievementView.Show(achievementEntries);
        }
    }
}