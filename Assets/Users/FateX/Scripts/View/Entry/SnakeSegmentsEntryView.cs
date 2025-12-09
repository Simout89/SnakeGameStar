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

        public void Init(SnakeSegmentEntry snakeSegmentEntry)
        {
            if (snakeSegmentEntry.SnakeSegmentSaveData.IsObtained)
            {
                image.sprite = snakeSegmentEntry.CardData.Sprite;
                name.text = snakeSegmentEntry.CardData.SnakeSegmentBase.UpgradeLevelsData.SegmentName;
                title.text = snakeSegmentEntry.CardData.Description;
            }
            else
            {
                
            }
        }
    }
}