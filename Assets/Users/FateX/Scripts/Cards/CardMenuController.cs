using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Users.FateX.Scripts.Data;
using Users.FateX.Scripts.Upgrade;
using Zenject;
using Скриптерсы.Services;
using Random = UnityEngine.Random;

namespace Users.FateX.Scripts.Cards
{
    public class CardMenuController : IInitializable, IDisposable
    {
        [Inject] private GameConfig _gameConfig;
        [Inject] private CardMenuView _cardMenuView;
        [Inject] private GameContext _gameContext;
        [Inject] private PlayerStats _playerStats;
        [Inject] private CurrencyService _currencyService;

        private int rerrolUsed = 0;
        private int exileUsed = 0;
        public event Action OnCardSelected;

        private List<CardData> _lastShownCards = new List<CardData>();
        private List<CardData> _exiledCards = new List<CardData>();

        private bool _exiledModeBacking;

        // НОВОЕ: Шанс появления карты улучшения (от 0 до 1)
        private const float UPGRADE_CARD_CHANCE = 0.5f; // 50% шанс

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
            if (_playerStats.Exile.Sum - exileUsed <= 0)
                return;
            _exiledMode = !_exiledMode;
        }

        private void HandleRerollClick()
        {
            if (_playerStats.Rerolls.Sum <= rerrolUsed)
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

            var availableSegmentCards = GetAvailableSegmentCards(isReroll);
            availableSegmentCards = availableSegmentCards.OrderBy(x => Random.value).ToList();
            var upgradeableSegments = GetUpgradeableSegments();

            List<CardData> cardsToShow = new List<CardData>();
            List<SnakeSegmentBase> usedSegments = new List<SnakeSegmentBase>();

            // 1. Пробуем добавить карты улучшений сегментов с шансом
            foreach (var segment in upgradeableSegments)
            {
                if (Random.value <= UPGRADE_CARD_CHANCE)
                {
                    usedSegments.Add(segment);
                    cardsToShow.Add(_gameConfig.GameConfigData.SpecialCards.UpgradeCard);
                    if (cardsToShow.Count >= cardsCount) break;
                }
            }

            // 2. Добавляем доступные сегменты, которых ещё нет
            foreach (var card in availableSegmentCards)
            {
                if (cardsToShow.Count >= cardsCount) break;
                if (!cardsToShow.Contains(card))
                    cardsToShow.Add(card);
            }

            // 3. Проверяем оставшиеся пустые слоты и добиваем картами улучшений
            var remainingUpgradeable = upgradeableSegments.Except(usedSegments).ToList();
            while (cardsToShow.Count < cardsCount && remainingUpgradeable.Count > 0)
            {
                var segment = remainingUpgradeable[0];
                usedSegments.Add(segment);
                remainingUpgradeable.RemoveAt(0);
                cardsToShow.Add(_gameConfig.GameConfigData.SpecialCards.UpgradeCard);
            }

            // 4. Если после всего остались пустые слоты — добавляем Heal/Coin
            while (cardsToShow.Count < cardsCount)
            {
                cardsToShow.Add(cardsToShow.Count % 2 == 0
                    ? _gameConfig.GameConfigData.SpecialCards.HealCard
                    : _gameConfig.GameConfigData.SpecialCards.CoinCard);
            }

            // Перемешиваем финальный список карт
            cardsToShow = cardsToShow.OrderBy(x => Random.value).ToList();

            _cardMenuView.ClearAllCards();

            // Отображаем карты
            for (int i = 0; i < cardsToShow.Count; i++)
            {
                var card = cardsToShow[i];

                if (card.CardType == CardType.Upgrade)
                {
                    var segment = usedSegments[0];
                    usedSegments.RemoveAt(0);
                    string statsText = GenerateUpgradeStatsText(segment);
                    ShowUpgradeCard(card, segment, statsText);
                }
                else if (card.CardType == CardType.Segment)
                {
                    ProcessSegmentCard(card);
                }
                else if (card.CardType == CardType.Heal)
                {
                    ProcessHealCard(card);
                }
                else if (card.CardType == CardType.Coin)
                {
                    ProcessCoinCard(card);
                }
            }

            _lastShownCards = cardsToShow.Where(c => c.CardType == CardType.Segment).ToList();
            _exiledMode = false;

            _cardMenuView.UpdateRerollCountText(_playerStats.Rerolls.Sum - rerrolUsed);
            _cardMenuView.UpdateExileCountText(_playerStats.Exile.Sum - exileUsed);
        }


        private List<CardData> GetAvailableSegmentCards(bool isReroll)
        {
            List<CardData> allSegmentCards = _gameConfig.GameConfigData.CardDatas
                .Where(c => c.CardType == CardType.Segment)
                .ToList();

            List<CardData> availableCards = new List<CardData>();

            foreach (var card in allSegmentCards)
            {
                if (_exiledCards.Contains(card))
                    continue;

                if (isReroll && _lastShownCards.Contains(card))
                    continue;

                int currentCount = _gameContext.SnakeController.SegmentsBase
                    .Count(s => s.GetType() == card.SnakeSegmentBase.GetType());

                if (currentCount < card.SnakeSegmentBase.UpgradeLevelsData.BaseSegmentsCount)
                {
                    availableCards.Add(card);
                }
            }

            return availableCards;
        }

        private List<SnakeSegmentBase> GetUpgradeableSegments()
        {
            var segments = _gameContext.SnakeController.SegmentsBase.ToArray();

            if (segments.Length <= 1)
                return new List<SnakeSegmentBase>();

            List<SnakeSegmentBase> upgradeableSegments = new List<SnakeSegmentBase>();

            for (int i = 1; i < segments.Length; i++)
            {
                var segment = segments[i];
                if (CanUpgradeSegment(segment))
                {
                    upgradeableSegments.Add(segment);
                }
            }

            return upgradeableSegments.OrderBy(x => Random.value).ToList();
        }

        private bool CanUpgradeSegment(SnakeSegmentBase segment)
        {
            return segment.CurrentLevel + 1 < segment.UpgradeLevelsData.UpgradeStats.Length;
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
                    _cardMenuView.UpdateExileCountText(_playerStats.Exile.Sum - exileUsed);
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

        private void ProcessHealCard(CardData card)
        {
            _cardMenuView.ShowCard(card, $"Восполняет {card.Value} здоровья").Button.onClick.AddListener(() =>
            {
                if (_exiledMode)
                {
                    _exiledMode = false;
                    return;
                }

                _gameContext.SnakeHealth.Heal(card.Value);
                _cardMenuView.ClearAllCards();
                _lastShownCards.Clear();
                OnCardSelected?.Invoke();
            });
        }

        private void ProcessCoinCard(CardData card)
        {
            _cardMenuView.ShowCard(card, $"Дает {card.Value} монет").Button.onClick.AddListener(() =>
            {
                if (_exiledMode)
                {
                    _exiledMode = false;
                    return;
                }

                _currencyService.AddCoins((int)card.Value);
                _cardMenuView.ClearAllCards();
                _lastShownCards.Clear();
                OnCardSelected?.Invoke();
            });
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
            _cardMenuView.ShowCardUpgradeCard(statsText, segment.UpgradeLevelsData.SegmentIcon).Button.onClick.AddListener(() =>
            {
                HandleClickOnCard(card, segment);
            });
        }

        private void HandleClickOnCard(CardData card, SnakeSegmentBase segment)
        {
            if (_exiledMode)
            {
                _exiledMode = false;
                return;
            }

            segment.Upgrade();
            _cardMenuView.ClearAllCards();
            _lastShownCards.Clear();
            OnCardSelected?.Invoke();
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
    }
}