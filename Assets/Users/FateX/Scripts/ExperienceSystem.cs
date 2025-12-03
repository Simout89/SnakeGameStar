using System;
using UnityEngine;
using Users.FateX.Scripts.CollectableItem;
using Zenject;

namespace Users.FateX.Scripts
{
    public class ExperienceSystem : IInitializable
    {
        private float BaseXpValue = 5f;
        public float NextLevelXp { get; private set; } = 5;
        public float CurrentXp { get; private set; }
        public float CurrentLevel { get; private set; } = 0;

        public event Action OnGetLevel;
        public event Action OnChangeXp;

        public void AddExperiencePoints(IExperiencePoints experiencePoints)
        {
            CurrentXp += experiencePoints.Value;
            OnChangeXp?.Invoke();

            // Обрабатываем ситуацию, когда опыта больше, чем нужно для уровня
            while (CurrentXp >= NextLevelXp)
            {
                UpLevel();
            }
        }
        
        public void AddExperiencePoints(float value)
        {
            CurrentXp += value;
            OnChangeXp?.Invoke();

            // Обрабатываем ситуацию, когда опыта больше, чем нужно для уровня
            while (CurrentXp >= NextLevelXp)
            {
                UpLevel();
            }
        }

        private void UpLevel()
        {
            CurrentLevel++;
            CurrentXp -= NextLevelXp; // оставшийся опыт переносим на следующий уровень
            UpdateLevelXpRequirement();
            OnGetLevel?.Invoke();
        }

        private void UpdateLevelXpRequirement()
        {
            NextLevelXp += BaseXpValue * 0.5f;
        }

        public void Initialize()
        {
            NextLevelXp = BaseXpValue;
        }
    }
}