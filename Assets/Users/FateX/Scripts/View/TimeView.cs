using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Users.FateX.Scripts.View
{
    public class TimeView: MonoBehaviour
    {
        [Inject] private GameTimer _gameTimer;
        [SerializeField] private TMP_Text _text;

        private void OnEnable()
        {
            _gameTimer.OnSecondChanged += HandleSecondChanged;
        }
        
        private void OnDisable()
        {
            _gameTimer.OnSecondChanged -= HandleSecondChanged;
        }

        private void HandleSecondChanged(int obj)
        {
            int minutes = obj / 60;
            int seconds = obj % 60;
            _text.text = $"{minutes:D2}:{seconds:D2}";
        }
    }
}