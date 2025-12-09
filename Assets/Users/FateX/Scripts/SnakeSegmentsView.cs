using System;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts
{
    public class SnakeSegmentsView: MonoBehaviour
    {
        [Inject] private SnakeSegmentsRepository _snakeSegmentsRepository;
        [Inject] private GameConfig _gameConfig;
        [SerializeField] private Transform container;

        public void OnEnable()
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }

            foreach (var segment in _snakeSegmentsRepository.SegmentEntries)
            {
                Instantiate(_gameConfig.GameConfigData.SnakeSegmentsEntryView, container).Init(segment);
            }
        }
    }
}