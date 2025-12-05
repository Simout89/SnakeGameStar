using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace Users.FateX.Scripts.View
{
    public class RoundCoinView : MonoBehaviour
    {
        [Inject] private RoundCurrency _roundCurrency;

        [SerializeField] private TMP_Text _text;

        private Sequence _seq;

        private void OnEnable()
        {
            _roundCurrency.OnCoinChanged += HandleCoinChanged;
            UpdateText(_roundCurrency.Coin);
        }

        private void OnDisable()
        {
            _roundCurrency.OnCoinChanged -= HandleCoinChanged;
        }

        private void HandleCoinChanged(int value)
        {
            UpdateText(value);
        }

        private void UpdateText(int value)
        {
            _text.text = "<sprite index=0>" + value.ToString();

            _seq?.Kill();

            _text.transform.localScale = Vector3.one;

            _seq = DOTween.Sequence()
                .Append(_text.transform.DOScale(1.2f, 0.12f).SetEase(Ease.OutCubic))
                .Append(_text.transform.DOScale(1f, 0.12f).SetEase(Ease.InCubic));
        }
    }
}