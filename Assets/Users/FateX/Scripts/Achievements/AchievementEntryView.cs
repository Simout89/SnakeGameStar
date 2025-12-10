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
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private GameObject CheckMark;
        
        public void Init(AchievementEntry achievementEntry)
        {
            _image.sprite = achievementEntry.AchievementData.Icon;
            name.text = achievementEntry.AchievementData.Name;
            title.text = DigitColorizer.ColorDigits(achievementEntry.AchievementData.Title, Color.yellow);

            CheckMark.SetActive(achievementEntry.AchievementSaveData.IsCompleted);


            progressText.text = achievementEntry.AchievementSaveData.Progress + "/" +
                                achievementEntry.AchievementData.RequiredValue;
        }
    }
}