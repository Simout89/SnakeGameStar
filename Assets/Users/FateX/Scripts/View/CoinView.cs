using System;
using TMPro;
using UnityEngine;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.View
{
    public class CoinView: MonoBehaviour
    {
        [Inject] private ICurrencyService _currencyService;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private bool showOnlyCoinCount;

        private void OnEnable()
        {
            _currencyService.OnCoinsChanged += HandleCoinChanged;
            UpdateText(_currencyService.Coins);
        }

        private void OnDisable()
        {
            _currencyService.OnCoinsChanged -= HandleCoinChanged;
        }

        private void HandleCoinChanged(int obj)
        {
            UpdateText(obj);
        }

        private void UpdateText(int obj)
        {
            if (showOnlyCoinCount)
            {
                _text.text = $"{obj}";
            }else
            {
                _text.text = $"Монет: {obj}";
            }
        }
    }
}