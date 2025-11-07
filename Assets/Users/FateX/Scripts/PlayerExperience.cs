using System;
using UnityEngine;

namespace Users.FateX.Scripts
{
    public class PlayerExperience: MonoBehaviour
    {
        private float nextLevelXp;
        private float currentXp;
        private float currentLevel = 1;

        public event Action OnGetLevel;

        public void AddXp(float value) // добавить модификаторы
        {
            currentXp += value;

            if (currentXp >= nextLevelXp)
            {
                UpdateLevelXpRequirement();
            }
        }

        private void UpdateLevelXpRequirement()
        {
            
        }
    }
}