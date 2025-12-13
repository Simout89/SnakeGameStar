using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Users.FateX.Scripts.Data;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Users.FateX.Scripts.Cards
{
    public class CardEntryView : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _cardName;
        [SerializeField] private Transform body;
        [SerializeField] private GameObject upgradeIcon;

        public void Init(CardData cardData)
        {
            _image.sprite = cardData.Sprite;
        }

        public void Init(CardData cardData, string text)
        {
            _text.text = text;

            _text.lineSpacing = 15f;

            if (cardData.SnakeSegmentBase != null)
                _cardName.text = cardData.SnakeSegmentBase.UpgradeLevelsData.LocalizedName.GetLocalizedString();

            _image.sprite = cardData.Sprite;

            body.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutCirc);
        }

        public void InitUpgradeCard(string text, Sprite sprite)
        {
            _text.text = text;

            _image.sprite = sprite;

            upgradeIcon.SetActive(true);

            body.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutCirc);
        }

        public void OnDestroy()
        {
            body.transform.DOKill();
        }
    }
}