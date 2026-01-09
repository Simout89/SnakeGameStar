using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Users.FateX.Scripts.View.Entry
{
    public class SnakeSegmentsEntryView: MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text title;

        private SnakeSegmentEntry snakeSegmentEntry;

        public void Init(SnakeSegmentEntry snakeSegmentEntry)
        {
            if (snakeSegmentEntry.SnakeSegmentSaveData.IsObtained)
            {
                image.sprite = snakeSegmentEntry.CardData.Sprite;
                name.text = snakeSegmentEntry.CardData.SnakeSegmentBase.UpgradeLevelsData.LocalizedName.GetLocalizedString();
                title.text = snakeSegmentEntry.CardData.LocalizedDescription.GetLocalizedString();
            }
            else
            {
                
            }

            this.snakeSegmentEntry = snakeSegmentEntry;
            snakeSegmentEntry.CardData.SnakeSegmentBase.UpgradeLevelsData.LocalizedName.StringChanged += Changed;
        }

        private void OnDestroy()
        {
            snakeSegmentEntry.CardData.SnakeSegmentBase.UpgradeLevelsData.LocalizedName.StringChanged -= Changed; 
        }

        private void Changed(string value)
        {
            if (snakeSegmentEntry.SnakeSegmentSaveData.IsObtained)
            {
                name.text = value;
                title.text = snakeSegmentEntry.CardData.LocalizedDescription.GetLocalizedString();
            }
        }
    }
}