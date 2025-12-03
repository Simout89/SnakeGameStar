using TMPro;
using UnityEngine;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.View
{
    public class RoundCoinView: MonoBehaviour
    {
        [Inject] private RoundCurrency _roundCurrency;

        [SerializeField] private TMP_Text _text;

        private void OnEnable()
        {
            _roundCurrency.OnCoinChanged += HandleCoinChanged;
            UpdateText(_roundCurrency.Coin);
        }

        private void OnDisable()
        {
            _roundCurrency.OnCoinChanged -= HandleCoinChanged;
        }

        private void HandleCoinChanged(int obj)
        {
            UpdateText(obj);
        }

        private void UpdateText(int i)
        {
            _text.text = i.ToString();
        }
    }
}