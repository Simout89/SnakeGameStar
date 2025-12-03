using System;
using DG.Tweening;
using UnityEngine;
using Users.FateX.Scripts.Data.Upgrade;

namespace Users.FateX.Scripts.Upgrade
{
    public class SnakeSegmentBase: MonoBehaviour, ITickable
    {
        [field: SerializeField] public  Transform Body { get; private set; }
        [field: SerializeField] public  Transform AdditionalParts { get; private set; }
        [field: SerializeField] public TriggerDetector CollectableTrigger { get; private set; }
        [field: SerializeField] public SnakeBodyPartHealth SnakeBodyPartHealth { get; private set; }
        [SerializeField] private SpriteRenderer[] snakeSprites;
        [Header("Data")]
        [SerializeField] protected UpgradeLevelsData upgradeLevelsData;

        public UpgradeLevelsData UpgradeLevelsData => upgradeLevelsData;

        private int currentLevel = 0;
        public int CurrentLevel => currentLevel;
        private float timeToNextShot;
        
        protected UpgradeStats CurrentStats;
        protected SnakeController SnakeController;
        
        public virtual void Init(SnakeController snakeController)
        {
            if(upgradeLevelsData != null)
            {
                CurrentStats = upgradeLevelsData.UpgradeStats[currentLevel];
            }
            SnakeController = snakeController;
            CollectableTrigger.transform.localScale += CollectableTrigger.transform.localScale *
                                                       snakeController.PlayerStats.PickUpRange.Sum;
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

        public void DamageEffect()
        {
            foreach (var sprite in snakeSprites)
            {
                sprite.DOComplete();
                
                sprite.material.SetFloat("_HitEffectBlend", 1f);
                sprite.material.DOFloat(0f, "_HitEffectBlend", 0.15f);
            }
        }
    }
    
    
    public interface ITickable
    {
        void Tick();
    }
}   