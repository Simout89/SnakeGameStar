using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Upgrade;
using Zenject;

namespace Users.FateX.Scripts.DebugUtils
{
    public class DebugSegmentSpawner: MonoBehaviour
    {
        [Inject] private GameContext _gameContext;

        [SerializeField] private SnakeSegmentBase[] _snakeSegmentBase;
        [SerializeField] private Transform container;
        [SerializeField] private Button _buttonPrefab;
        
        private void Awake()
        {
            for (int i = 0; i < _snakeSegmentBase.Length; i++)
            {
                var newButton = Instantiate(_buttonPrefab, container);
                int value = i;
                newButton.onClick.AddListener(() => HandleClick(value));
                newButton.transform.GetChild(0).GetComponent<TMP_Text>().text = _snakeSegmentBase[i].name;
            }
        }

        private void HandleClick(int value)
        {
            _gameContext.SnakeController.Grow(_snakeSegmentBase[value]);
        }
    }
}