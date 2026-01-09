using System.Collections.Generic;
using UnityEngine;

namespace Users.FateX.Scripts.Achievements
{
    public class AchievementModel
    {
        public Dictionary<string, AchievementEntry> AchievementEntries { get; private set; }

        public AchievementModel(Dictionary<string, AchievementEntry> achievementEntries)
        {
            AchievementEntries = achievementEntries;
            
            Debug.Log(AchievementEntries.Count);
        }
    }
    
    public class AchievementEntry
    {
        public AchievementData AchievementData { get; }
        public AchievementSaveData AchievementSaveData { get; }

        public AchievementEntry(AchievementData data, AchievementSaveData saveData)
        {
            AchievementData = data;
            AchievementSaveData = saveData;
        }
    }
}