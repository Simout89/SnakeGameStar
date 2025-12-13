using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
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

        private AchievementEntry _achievementEntry;
        
        public void Init(AchievementEntry achievementEntry)
        {
            _image.sprite = achievementEntry.AchievementData.Icon;
            name.text = achievementEntry.AchievementData.LocalizedName.GetLocalizedString();
            title.text = DigitColorizer.ColorDigits(achievementEntry.AchievementData.LocalizedDescription.GetLocalizedString(), Color.yellow);

            CheckMark.SetActive(achievementEntry.AchievementSaveData.IsCompleted);


            progressText.text = achievementEntry.AchievementSaveData.Progress + "/" +
                                achievementEntry.AchievementData.RequiredValue;

            _achievementEntry = achievementEntry;
            _achievementEntry.AchievementData.LocalizedName.StringChanged += Changed;
        }

        private void OnDisable()
        {
            _achievementEntry.AchievementData.LocalizedName.StringChanged -= Changed;
        }

        private void Changed(string obj)
        {
            name.text = _achievementEntry.AchievementData.LocalizedName.GetLocalizedString();
            title.text = DigitColorizer.ColorDigits(_achievementEntry.AchievementData.LocalizedDescription.GetLocalizedString(), Color.yellow);
        }
    }
}