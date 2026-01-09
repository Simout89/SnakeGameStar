using System;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts.Achievements
{
    public class AchievementView: MonoBehaviour, IAchievementView
    {
        [Inject] private GameConfig _gameConfig;
        [SerializeField] private Transform container;
        public void Show(AchievementEntry[] achievementEntry)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            foreach (var achievement in achievementEntry)
            {
                var newEntry = Instantiate(_gameConfig.GameConfigData.AchievementEntryView, container);
                newEntry.Init(achievement);
            }
        }

        public event Action OnUpdate;

        private void OnEnable()
        {
            OnUpdate?.Invoke();
        }
    }

    public interface IAchievementView
    {
        public void Show(AchievementEntry[] achievementEntry);
        public event Action OnUpdate;
    }
}