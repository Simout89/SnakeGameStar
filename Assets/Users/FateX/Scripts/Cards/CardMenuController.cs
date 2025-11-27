using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Upgrade;
using Zenject;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.Cards
{
    public class CardMenuController : IInitializable, IDisposable
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private CardMenuView _cardMenuView;
        [Inject] private GameContext _gameContext;
        [Inject] private PlayerStats _playerStats;

        private int rerrolUsed = 0;
        private int exileUsed = 0;
        public event Action OnCardSelected;

        private List<CardData> _lastShownCards = new List<CardData>();
        private List<CardData> _exiledCards = new List<CardData>();

        private bool _exiledModeBacking;

        private bool _exiledMode
        {
            get => _exiledModeBacking;
            set
            {
                _exiledModeBacking = value;          
                _cardMenuView.ChangeExileMode(value);
            }
        }

        public void Initialize()
        {
            _cardMenuView.RerollButton.onClick.AddListener(HandleRerollClick);
            _cardMenuView.ExileButton.onClick.AddListener(HandleExileClick);
        }

        public void Dispose()
        {
            _cardMenuView.RerollButton.onClick.RemoveListener(HandleRerollClick);
            _cardMenuView.ExileButton.onClick.RemoveListener(HandleExileClick);
        }

        private void HandleExileClick()
        {
            if(_playerStats.exile - exileUsed <= 0)
                return;
            _exiledMode = !_exiledMode;
        }

        private void HandleRerollClick()
        {
            if (_playerStats.rerolls <= rerrolUsed)
            {
                return;
            }

            rerrolUsed++;
            _cardMenuView.ClearAllCards();
            SpawnRandomCards(3, true);
        }

        public void SpawnRandomCards(int cardsCount = 3, bool isReroll = false)
        {
            if (cardsCount <= 0) return;

            List<CardData> cardDatas = new List<CardData>(_gameConfig.GameConfigData.CardDatas);
            if (cardDatas.Count == 0) return;

            List<CardData> availableCards;

            // Фильтруем только если это рерол
            if (isReroll && _lastShownCards.Count > 0)
            {
                // Удаляем последние показанные карты из доступных, если есть альтернативы
                availableCards = cardDatas.Where(card => !_lastShownCards.Contains(card)).ToList();

                // Если после фильтрации осталось мало карт, используем все доступные
                if (availableCards.Count < cardsCount)
                {
                    availableCards = new List<CardData>(cardDatas);
                }
            }
            else
            {
                availableCards = new List<CardData>(cardDatas);
            }

            int attempts = 0;
            int maxAttempts = cardsCount * 10;
            List<CardData> currentlyShownCards = new List<CardData>();

            for (int i = 0; i < cardsCount && attempts < maxAttempts; i++)
            {
                attempts++;

                if (availableCards.Count == 0) break;

                var currentCard = availableCards[Random.Range(0, availableCards.Count)];

                if (!CanProcessCard(currentCard))
                {
                    availableCards.Remove(currentCard);
                    i--;
                    continue;
                }

                if (currentCard.CardType == CardType.Segment)
                {
                    ProcessSegmentCard(currentCard);
                    currentlyShownCards.Add(currentCard);
                    availableCards.Remove(currentCard);
                }
                else if (currentCard.CardType == CardType.Upgrade)
                {
                    if (!TryProcessUpgradeCard(currentCard))
                    {
                        availableCards.Remove(currentCard);
                        i--;
                        continue;
                    }

                    currentlyShownCards.Add(currentCard);
                    availableCards.Remove(currentCard);
                }
                else
                {
                    availableCards.Remove(currentCard);
                }
            }

            // Сохраняем текущие показанные карты для следующего рерола
            _lastShownCards = currentlyShownCards;

            _exiledMode = false;
            
            _cardMenuView.UpdateRerollCountText(_playerStats.rerolls - rerrolUsed);
            _cardMenuView.UpdateExileCountText(_playerStats.exile - exileUsed);

        }

        private bool CanProcessCard(CardData card)
        {
            foreach (var exiledCard in _exiledCards)
            {
                if (card == exiledCard)
                    return false;
            }

            if (card.CardType == CardType.Upgrade &&
                _gameContext.SnakeController.SegmentsBase.Count <= 1)
            {
                return false;
            }

            if (card.CardType == CardType.Segment)
            {
                int count = 0;

                foreach (var segment in _gameContext.SnakeController.SegmentsBase)
                {
                    if (card.SnakeSegmentBase.GetType() == segment.GetType())
                    {
                        count++;
                    }
                }

                if (count >= card.SnakeSegmentBase.UpgradeLevelsData.BaseSegmentsCount)
                {
                    return false;
                }
            }

            return true;
        }

        private void ProcessSegmentCard(CardData card)
        {
            _cardMenuView.ShowCard(card).Button.onClick.AddListener(() =>
            {
                if (_exiledMode)
                {
                    _exiledCards.Add(card);
                    _cardMenuView.ClearAllCards();
                    _exiledMode = false;
                    exileUsed++;
                    _cardMenuView.UpdateExileCountText(_playerStats.exile - exileUsed);
                    OnCardSelected?.Invoke();
                }
                else
                {
                    _gameContext.SnakeController.Grow(card.SnakeSegmentBase);
                    _cardMenuView.ClearAllCards();
                    _lastShownCards.Clear();
                    OnCardSelected?.Invoke();
                }
            });
        }

        private bool TryProcessUpgradeCard(CardData card)
        {
            var segments = _gameContext.SnakeController.SegmentsBase.ToArray();
            if (segments.Length <= 1)
            {
                return false;
            }

            SnakeSegmentBase segmentToUpgrade = FindUpgradeableSegment(segments);
            if (segmentToUpgrade == null)
            {
                return false;
            }

            string statsText = GenerateUpgradeStatsText(segmentToUpgrade);
            ShowUpgradeCard(card, segmentToUpgrade, statsText);

            return true;
        }

        private SnakeSegmentBase FindUpgradeableSegment(SnakeSegmentBase[] segments)
        {
            List<int> triedIndices = new List<int>();

            while (triedIndices.Count < segments.Length - 1)
            {
                int randomIndex = Random.Range(1, segments.Length);
                if (triedIndices.Contains(randomIndex)) continue;

                triedIndices.Add(randomIndex);
                var candidate = segments[randomIndex];

                if (CanUpgradeSegment(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }

        private bool CanUpgradeSegment(SnakeSegmentBase segment)
        {
            return segment.CurrentLevel + 1 < segment.UpgradeLevelsData.UpgradeStats.Length;
        }

        private string GenerateUpgradeStatsText(SnakeSegmentBase segment)
        {
            var data = segment.UpgradeLevelsData.UpgradeStats;
            var currentStats = data[segment.CurrentLevel];
            var nextStats = data[segment.CurrentLevel + 1];

            string statsText = $"Улучшение: {segment.UpgradeLevelsData.SegmentName}\n";
            statsText += GetStatDifference(currentStats.DelayBetweenShots * 10, nextStats.DelayBetweenShots * 10,
                "AttackSpeed", true);
            statsText += GetStatDifference(currentStats.Damage * GameConstant.VisualDamageMultiplayer,
                nextStats.Damage * GameConstant.VisualDamageMultiplayer, "Damage");
            statsText += GetStatDifference(currentStats.Duration, nextStats.Duration, "Duration");
            statsText += GetStatDifference(currentStats.AttackRange, nextStats.AttackRange, "AttackRange");
            statsText += GetStatDifference(currentStats.BouncesCount, nextStats.BouncesCount, "BouncesCount");
            statsText += GetStatDifference(currentStats.DamageArea, nextStats.DamageArea, "DamageArea");
            statsText += GetStatDifference(currentStats.ProjectileCount, nextStats.ProjectileCount,
                "ProjectileCount");

            return statsText;
        }

        private void ShowUpgradeCard(CardData card, SnakeSegmentBase segment, string statsText)
        {
            _cardMenuView.ShowCard(card, statsText).Button.onClick.AddListener(() =>
            {
                HandleClickOnCard(card, segment);
            });
        }

        private void HandleClickOnCard(CardData card, SnakeSegmentBase segment)
        {
            if (_exiledMode)
            {
                
            }
            else
            {
                segment.Upgrade();
                _cardMenuView.ClearAllCards();
                _lastShownCards.Clear();
                OnCardSelected?.Invoke();
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
            _lastShownCards.Clear(); // Очищаем историю при выборе карты
            OnCardSelected?.Invoke();
        }
    }
}