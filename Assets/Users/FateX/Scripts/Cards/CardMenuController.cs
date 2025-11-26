using System;
using System.Collections.Generic;
using System.Linq;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Upgrade;
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
            if (cardsCount <= 0) return;

            List<CardData> cardDatas = new List<CardData>(_gameConfig.GameConfigData.CardDatas);
            if (cardDatas.Count == 0) return;

            int attempts = 0;
            int maxAttempts = cardsCount * 10;

            for (int i = 0; i < cardsCount && attempts < maxAttempts; i++)
            {
                attempts++;

                if (cardDatas.Count == 0) break;

                var currentCard = cardDatas[Random.Range(0, cardDatas.Count)];

                if (currentCard.CardType == CardType.Upgrade &&
                    _gameContext.SnakeController.SegmentsBase.Count <= 1)
                {
                    cardDatas.Remove(currentCard);
                    i--;
                    continue;
                }

                if (currentCard.CardType == CardType.Segment)
                {
                    _cardMenuView.ShowCard(currentCard).Button.onClick.AddListener(() =>
                    {
                        _gameContext.SnakeController.Grow(currentCard.SnakeSegmentBase);
                        _cardMenuView.ClearAllCards();
                        OnCardSelected?.Invoke();
                    });

                    cardDatas.Remove(currentCard);
                }
                else if (currentCard.CardType == CardType.Upgrade)
                {
                    var segments = _gameContext.SnakeController.SegmentsBase.ToArray();
                    if (segments.Length <= 1)
                    {
                        cardDatas.Remove(currentCard);
                        i--;
                        continue;
                    }

                    // Ищем сегмент который можно улучшить
                    SnakeSegmentBase snakeSegmentBase = null;
                    List<int> triedIndices = new List<int>();

                    while (triedIndices.Count < segments.Length - 1)
                    {
                        int randomIndex = Random.Range(1, segments.Length);
                        if (triedIndices.Contains(randomIndex)) continue;

                        triedIndices.Add(randomIndex);
                        var candidate = segments[randomIndex];

                        // Проверяем, можно ли улучшить этот сегмент
                        if (candidate.CurrentLevel + 1 < candidate.UpgradeLevelsData.UpgradeStats.Length)
                        {
                            snakeSegmentBase = candidate;
                            break;
                        }
                    }

                    // Если все сегменты на максимуме
                    if (snakeSegmentBase == null)
                    {
                        cardDatas.Remove(currentCard);
                        i--;
                        continue;
                    }

                    var data = snakeSegmentBase.UpgradeLevelsData.UpgradeStats;
                    var currentStats = data[snakeSegmentBase.CurrentLevel];
                    var nextStats = data[snakeSegmentBase.CurrentLevel + 1];

                    string statsText = $"Улучшение: {snakeSegmentBase.UpgradeLevelsData.SegmentName}\n";
                    statsText += GetStatDifference(currentStats.DelayBetweenShots * 10, nextStats.DelayBetweenShots * 10,
                        "AttackSpeed", true);
                    statsText += GetStatDifference(currentStats.Damage * GameConstant.VisualDamageMultiplayer, nextStats.Damage * GameConstant.VisualDamageMultiplayer, "Damage");
                    statsText += GetStatDifference(currentStats.Duration, nextStats.Duration, "Duration");
                    statsText += GetStatDifference(currentStats.AttackRange, nextStats.AttackRange, "AttackRange");
                    statsText += GetStatDifference(currentStats.BouncesCount, nextStats.BouncesCount, "BouncesCount");
                    statsText += GetStatDifference(currentStats.DamageArea, nextStats.DamageArea, "DamageArea");
                    statsText += GetStatDifference(currentStats.ProjectileCount, nextStats.ProjectileCount,
                        "ProjectileCount");

                    _cardMenuView.ShowCard(currentCard, statsText).Button.onClick.AddListener(() =>
                    {
                        snakeSegmentBase.Upgrade();
                        _cardMenuView.ClearAllCards();
                        OnCardSelected?.Invoke();
                    });

                    cardDatas.Remove(currentCard);
                }
                else
                {
                    cardDatas.Remove(currentCard);
                }
            }
        }


        private string GetStatDifference(float currentValue, float nextValue, string statName, bool invertValue = false)
        {
            if (currentValue != nextValue)
            {
                if (invertValue)
                {
                    return $"{statName} +{currentValue - nextValue}\n";
                }
                else
                {
                    return $"{statName} +{nextValue - currentValue}\n";
                }
            }

            return string.Empty;
        }

        private void HandleClick(CardData cardData)
        {
            _gameContext.SnakeController.Grow(cardData.SnakeSegmentBase);

            _cardMenuView.ClearAllCards();

            OnCardSelected?.Invoke();
        }
    }
}