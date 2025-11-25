using UnityEngine;
using Users.FateX.Scripts.Data;
using Zenject;

namespace Users.FateX.Scripts.Cards
{
    public class CardMenuView: MonoBehaviour
    {
        [Inject] private GameConfig _gameConfig;
        
        [SerializeField] private Transform _cardContainer;
        [SerializeField] private GameObject backGround;
        
        public CardEntryView ShowCard(CardData cardData)
        {
            var newCard = Instantiate(_gameConfig.GameConfigData.CardPrefab, _cardContainer);

            newCard.Init(cardData, cardData.Description);

            backGround.SetActive(true);

            return newCard;
        }
        
        public CardEntryView ShowCard(CardData cardData, string text = "")
        {
            var newCard = Instantiate(_gameConfig.GameConfigData.CardPrefab, _cardContainer);

            newCard.Init(cardData, text);

            backGround.SetActive(true);

            return newCard;
        }
        
        

        public void ClearAllCards()
        {
            foreach (Transform child in _cardContainer)
            {
                Destroy(child.gameObject);
            }
            backGround.SetActive(false);
        }
    }
}