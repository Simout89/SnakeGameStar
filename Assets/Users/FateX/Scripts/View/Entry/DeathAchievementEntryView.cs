using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Achievements;

namespace Users.FateX.Scripts.View.Entry
{
    public class DeathAchievementEntryView: MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text prize;
        
        public void Init(AchievementEntry achievementEntry, GameConfig gameConfig)
        {
            _image.sprite = achievementEntry.AchievementData.Icon;
            name.text = achievementEntry.AchievementData.LocalizedName.GetLocalizedString();
            title.text = achievementEntry.AchievementData.LocalizedDescription.GetLocalizedString();
            prize.text = $"{gameConfig.GameConfigData.LocalizationData.Gained.GetLocalizedString()}: <color=white>{achievementEntry.AchievementData.CardData.SnakeSegmentBase.UpgradeLevelsData.LocalizedName.GetLocalizedString()}";
        }
    }
}