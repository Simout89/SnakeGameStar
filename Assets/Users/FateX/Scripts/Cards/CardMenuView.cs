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
        [field: SerializeField] public Button LockButton { get; private set; }
        [field: SerializeField] public Button ExileButton { get; private set; }

        public CardEntryView ShowCard(CardData cardData, string customText = null)
        {
            var newCard = Instantiate(_gameConfig.GameConfigData.CardPrefab, _cardContainer);
            
            string displayText = string.IsNullOrEmpty(customText) ? cardData.Description : customText;
            newCard.Init(cardData, displayText);

            SetBackgroundActive(true);

            return newCard;
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