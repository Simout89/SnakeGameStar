using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Utils;

namespace Users.FateX.Scripts.Achievements
{
    public class AchievementEntryView: MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text title;
        
        public void Init(AchievementEntry achievementEntry)
        {
            _image.sprite = achievementEntry.AchievementData.Icon;
            name.text = achievementEntry.AchievementData.Name;
            title.text = DigitColorizer.ColorDigits(achievementEntry.AchievementData.Title, Color.yellow);
        }
    }
}