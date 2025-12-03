using System;
using TMPro;
using UnityEngine;
using Users.FateX.Scripts.Utils;
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
            _text.text = MyUtils.FormatSeconds(obj);
        }
    }
}