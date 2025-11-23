using System;
using System.Collections.Generic;
using Users.FateX.Scripts.Data;
using Zenject;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.Cards
{
    public class CardMenuController
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private CardMenuView _cardMenuView;

        [Inject] private GameContext _gameContext;

        public event Action OnCardSelected;

        public void SpawnRandomCards(int cardsCount = 3)
        {
            List<CardData> cardDatas = new List<CardData>(_gameConfig.GameConfigData.CardDatas);

            for (int i = 0; i < cardsCount; i++)
            {
                var currentCard = cardDatas[Random.Range(0, cardDatas.Count)];

                _cardMenuView.ShowCard(currentCard).Button.onClick.AddListener(() => HandleClick(currentCard));

                cardDatas.Remove(currentCard);
            }
        }

        private void HandleClick(CardData cardData)
        {
            _gameContext.SnakeController.Grow(cardData.SnakeSegmentBase);

            _cardMenuView.ClearAllCards();
            
            OnCardSelected?.Invoke();
        }
    }
}