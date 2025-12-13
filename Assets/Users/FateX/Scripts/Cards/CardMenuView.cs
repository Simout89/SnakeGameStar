using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Data;
using Zenject;

namespace Users.FateX.Scripts.Cards
{
    public class CardMenuView : MonoBehaviour
    {
        [Inject] private GameConfig _gameConfig;
        
        [SerializeField] private Transform _cardContainer;
        [SerializeField] private GameObject _background;
        [field: SerializeField] public Button RerollButton { get; private set; }
        [field: SerializeField] public Button ExileButton { get; private set; }
        [SerializeField] private TMP_Text rerollCountText;
        [SerializeField] private TMP_Text exileCountText;
        [SerializeField] private Image exileImage;

        public CardEntryView ShowCard(CardData cardData, string customText = null)
        {
            var newCard = Instantiate(_gameConfig.GameConfigData.CardPrefab, _cardContainer);
            
            string displayText = string.IsNullOrEmpty(customText) ? cardData.LocalizedDescription.GetLocalizedString() : customText;
            newCard.Init(cardData, displayText);

            SetBackgroundActive(true);

            return newCard;
        }
        
        public CardEntryView ShowCardUpgradeCard(string customText , Sprite sprite)
        {
            var newCard = Instantiate(_gameConfig.GameConfigData.CardPrefab, _cardContainer);
            
            newCard.InitUpgradeCard(customText, sprite);

            SetBackgroundActive(true);

            return newCard;
        }

        public void ChangeExileMode(bool mode)
        {
            if (mode)
            {
                exileImage.color = Color.red;
            }
            else
            {
                exileImage.color = Color.white;
            }
        }
        
        public void RemoveCard(CardEntryView cardView)
        {
            if (cardView != null)
            {
                Destroy(cardView.gameObject);

                if (_cardContainer.childCount == 0)
                {
                    SetBackgroundActive(false);
                }
            }
        }


        public void UpdateRerollCountText(int count)
        {
            rerollCountText.text = count.ToString();
        }
        
        public void UpdateExileCountText(int count)
        {
            exileCountText.text = count.ToString();
        }

        public void ClearAllCards()
        {
            DestroyAllChildren();
            SetBackgroundActive(false);
        }

        private void DestroyAllChildren()
        {
            foreach (Transform child in _cardContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void SetBackgroundActive(bool isActive)
        {
            if (_background != null)
            {
                _background.SetActive(isActive);
            }
        }
    }
    
    
}