using System;
using UnityEngine;
using Users.FateX.Scripts.CollectableItem;
using Zenject;

namespace Users.FateX.Scripts
{
    public class ExperienceSystem: IInitializable
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
            
            if (CurrentXp >= NextLevelXp)
            {
                UpLevel();
            }
        }

        private void UpLevel()
        {
            CurrentLevel++;
            CurrentXp = 0;
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