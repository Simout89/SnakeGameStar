using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Services;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.Achievements
{
    public class AchievementController : IInitializable, IDisposable
    {
        [Inject] private ISaveLoadService _saveLoadService;
        [Inject] private SnakeSegmentsRepository _snakeSegmentsRepository;

        [InjectOptional] private ExperienceSystem _experienceSystem;
        [InjectOptional] private IAchievementView _achievementView;
        [InjectOptional] private GameTimer _gameTimer;
        
        private AchievementModel _achievementModel;

        private List<AchievementEntry> _newObtained = new List<AchievementEntry>();

        public void Initialize()
        {
            _achievementModel = new AchievementModel(_saveLoadService.GetAchievements());
            
            GameEvents.OnDealDamage += HandleDealDamage;
            GameEvents.OnEnemyDie += HandleEnemyDie;

            if (_achievementView != null)
            {
                _achievementView.OnUpdate += HandleUpdateView;
            }

            if(_gameTimer != null)
                _gameTimer.OnSecondChanged += HandleSecondChanged;

            if (_experienceSystem != null)
                _experienceSystem.OnGetLevel += HandleGetLevel;

            foreach (var achievement in _achievementModel.AchievementEntries)
            {
                if (achievement.Value.AchievementSaveData.IsCompleted == true &&
                    achievement.Value.AchievementSaveData.IsRewarded == false)
                {
                    _snakeSegmentsRepository.ObtainSegment(achievement.Value.AchievementData.CardData);
                    achievement.Value.AchievementSaveData.IsRewarded = true;
                }
            }
        }

        private void HandleEnemyDie(EnemyData arg1, DamageInfo arg2)
        {
            foreach (var achievement in _achievementModel.AchievementEntries)
            {
                if (achievement.Value.AchievementData.AchievementType == AchievementType.Kill)
                {
                    AchievementAddProgress(achievement);
                }
                if (achievement.Value.AchievementData.AchievementType == AchievementType.KillWithWeapon &&
                          achievement.Value.AchievementData.SnakeSegmentBase.UpgradeLevelsData.SegmentName == arg2.DamageDealerName)
                {
                    AchievementAddProgress(achievement);
                }
            }
        }

        private void AchievementAddProgress(KeyValuePair<string, AchievementEntry> achievement)
        {
            if(achievement.Value.AchievementSaveData.IsCompleted)
                return;
                    
            achievement.Value.AchievementSaveData.Progress++;

            if (achievement.Value.AchievementSaveData.Progress >=
                achievement.Value.AchievementData.RequiredValue)
            {
                Debug.Log("Достижение выполнено");
                        
                achievement.Value.AchievementSaveData.IsCompleted = true;
                _newObtained.Add(achievement.Value);
            }
        }

        public AchievementEntry[] GetNewObtainedAchievement()
        {
            var array = _newObtained.ToArray();;
            return array;
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
            
            if(_gameTimer != null)
                _gameTimer.OnSecondChanged -= HandleSecondChanged;
            
            if (_experienceSystem != null)
                _experienceSystem.OnGetLevel -= HandleGetLevel;

            foreach (var obtained in _newObtained)
            {
                obtained.AchievementSaveData.IsRewarded = true;
                _snakeSegmentsRepository.ObtainSegment(obtained.AchievementData.CardData);
            }
            
            _saveLoadService.SaveAchievements(_achievementModel.AchievementEntries);


        }

        private void HandleGetLevel()
        {
            foreach (var achievement in _achievementModel.AchievementEntries)
            {
                if (achievement.Value.AchievementData.AchievementType == AchievementType.LevelUp)
                {
                    AchievementAddProgress(achievement);
                }
            }
        }

        private void HandleSecondChanged(int obj)
        {
            foreach (var achievement in _achievementModel.AchievementEntries)
            {
                if (achievement.Value.AchievementData.AchievementType == AchievementType.LiveTime)
                {
                    AchievementAddProgress(achievement);
                }
            }
        }

        private void HandleUpdateView()
        {
            AchievementEntry[] achievementEntries = _achievementModel.AchievementEntries.Values.ToArray();

            _achievementView.Show(achievementEntries);
        }
    }
}