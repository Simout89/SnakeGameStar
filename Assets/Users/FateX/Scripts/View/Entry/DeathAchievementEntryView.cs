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
        
        public void Init(AchievementEntry achievementEntry)
        {
            _image.sprite = achievementEntry.AchievementData.Icon;
            name.text = achievementEntry.AchievementData.Name;
            title.text = achievementEntry.AchievementData.Title;
            prize.text = $"Получено {achievementEntry.AchievementData.CardData.SnakeSegmentBase.UpgradeLevelsData.SegmentName}";
        }
    }
}