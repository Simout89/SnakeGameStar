using System.Collections.Generic;
using Unity.Services.Analytics;
using Users.FateX.Scripts;
using Users.FateX.Scripts.Analytics.Events;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Enemys;
using Users.FateX.Scripts.Utils;
using Zenject;

namespace Скриптерсы.Services
{
    public class StatisticsService : IStatisticsService
    {
        [Inject] private GameTimer gameTimer;
        [Inject] private RoundCurrency roundCurrency;
        [Inject] private EnemyDeathHandler enemyDeathHandler;
        [Inject] private ExperienceSystem experienceSystem;
        [Inject] private GameContext gameContext;
        [Inject] private CollectableHandler collectableHandler;
        [Inject] private GameConfig _gameConfig;

        public string[] GetStatistics()
        {
            List<string> statistic = new List<string>();
            statistic.Add(FormatStatisticLine($"{_gameConfig.GameConfigData.LocalizationData.TimeSurvived.GetLocalizedString()}:", MyUtils.FormatSeconds(gameTimer.CurrentTime)));
            statistic.Add(FormatStatisticLine($"{_gameConfig.GameConfigData.LocalizationData.CoinsEarned.GetLocalizedString()}:", roundCurrency.Coin.ToString()));
            statistic.Add(FormatStatisticLine($"{_gameConfig.GameConfigData.LocalizationData.EnemiesKilled.GetLocalizedString()}:", enemyDeathHandler.TotalEnemyDie.ToString()));
            statistic.Add(FormatStatisticLine($"{_gameConfig.GameConfigData.LocalizationData.LevelsGained.GetLocalizedString()}:", experienceSystem.CurrentLevel.ToString()));
            statistic.Add(FormatStatisticLine($"{_gameConfig.GameConfigData.LocalizationData.SegmentsGained.GetLocalizedString()}:", (gameContext.SnakeController.SegmentsBase.Count - 1).ToString()));
            statistic.Add(FormatStatisticLine($"{_gameConfig.GameConfigData.LocalizationData.ApplesEaten.GetLocalizedString()}:", collectableHandler.HealItemUsed.ToString()));
            statistic.Add(FormatStatisticLine($"{_gameConfig.GameConfigData.LocalizationData.MagnetsUsed.GetLocalizedString()}:", collectableHandler.MagnetItemUsed.ToString()));

            AnalyticsService.Instance.RecordEvent(
                new OnRunEndedEvent(
                    lifeTime: (int)gameTimer.CurrentTime,
                    segmentsCounts: (gameContext.SnakeController.SegmentsBase.Count - 1),
                    userLevel: (int)experienceSystem.CurrentLevel,
                    coinsEarned: roundCurrency.Coin,
                    onEnemyKilled: enemyDeathHandler.TotalEnemyDie
                )
            );
            
            return statistic.ToArray();
        }

        private string FormatStatisticLine(string label, string value)
        {
            return $"<align=left>{label}<line-height=0>\n<align=right><color=yellow>{value}</color><line-height=1em>";
        }
    }

    public interface IStatisticsService
    {
        public string[] GetStatistics();
    }
}