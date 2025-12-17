using System;
using System.Collections.Generic;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Services;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class SnakeSegmentsRepository : IInitializable, IDisposable
    {
        [Inject] private ISaveLoadService _saveLoadService;

        private List<SnakeSegmentEntry> segmentEntries = new List<SnakeSegmentEntry>();
        public IReadOnlyCollection<SnakeSegmentEntry> SegmentEntries => segmentEntries;

        public void ClearData()
        {
            segmentEntries = _saveLoadService.GetSegmentEntries();
        }

        public void Initialize()
        {
            segmentEntries = _saveLoadService.GetSegmentEntries();
        }

        public void Dispose()
        {
            _saveLoadService.SaveSegments(segmentEntries);
        }

        public void ObtainSegment(CardData cardData)
        {
            foreach (var segment in segmentEntries)
            {
                if (cardData.Id == segment.CardData.Id)
                {
                    segment.SnakeSegmentSaveData.IsObtained = true;
                }
            }
        }

        public CardData[] GetObtainedCardData()
        {
            List<CardData> obtained = new();

            foreach (var segment in segmentEntries)
            {
                if (segment.SnakeSegmentSaveData.IsObtained)
                {
                    obtained.Add(segment.CardData);
                }
            }

            return obtained.ToArray();
        }
    }

    public class SnakeSegmentEntry
    {
        public CardData CardData { get; }
        public SnakeSegmentSaveData SnakeSegmentSaveData { get; }

        public SnakeSegmentEntry(CardData cardData, SnakeSegmentSaveData snakeSegmentSaveData)
        {
            CardData = cardData;
            SnakeSegmentSaveData = snakeSegmentSaveData;
        }
    }

    public class SnakeSegmentSaveData
    {
        public string Id;
        public bool IsObtained;
        
        public SnakeSegmentSaveData() {}

        public SnakeSegmentSaveData(CardData cardData)
        {
            Id = cardData.Id;
            IsObtained = cardData.IsObtained;
        }
    }
}