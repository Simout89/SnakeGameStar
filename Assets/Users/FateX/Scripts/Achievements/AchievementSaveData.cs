using System;

namespace Users.FateX.Scripts.Achievements
{
    [Serializable]
    public class AchievementSaveData
    {
        public string Id;
        public float Progress;
        public bool IsCompleted;
        public bool IsRewarded;

        public AchievementSaveData()
        {
            
        }

        public AchievementSaveData(AchievementData achievementData)
        {
            Id = achievementData.Id;
        }
    }
}