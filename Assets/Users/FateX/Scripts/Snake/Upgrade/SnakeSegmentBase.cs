using UnityEngine;
using Users.FateX.Scripts.Data.Upgrade;

namespace Users.FateX.Scripts.Upgrade
{
    public class SnakeSegmentBase: MonoBehaviour, ITickable
    {
        [SerializeField] public Transform Body;
        [SerializeField] public Transform AdditionalParts;
        [Header("Data")]
        [SerializeField] protected UpgradeLevelsData upgradeLevelsData;

        private int currentLevel = 0;
        private float timeToNextShot;
        
        protected UpgradeStats CurrentStats;
        
        public virtual void Init()
        {
            CurrentStats = upgradeLevelsData.UpgradeStats[currentLevel];
        }

        public virtual void Tick()
        {
            
        }

        protected bool CheckOnTimeDone()
        {
            if (Time.time < timeToNextShot)
            {
                return false;
            }

            timeToNextShot = Time.time + CurrentStats.DelayBetweenShots;

            return true;
        }

        public virtual void Upgrade()
        {
            currentLevel++;
            CurrentStats = upgradeLevelsData.UpgradeStats[currentLevel];
        }
    }
    
    
    public interface ITickable
    {
        void Tick();
    }
}   