using System.Collections.Generic;
using Users.FateX.Scripts;
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

        public string[] GetStatistics()
        {
            List<string> statistic = new List<string>();
            statistic.Add(FormatStatisticLine("Прожил времени:", MyUtils.FormatSeconds(gameTimer.CurrentTime)));
            statistic.Add(FormatStatisticLine("Монет получено:", roundCurrency.Coin.ToString()));
            statistic.Add(FormatStatisticLine("Врагов убито:", enemyDeathHandler.TotalEnemyDie.ToString()));
            statistic.Add(FormatStatisticLine("Уровней получено:", experienceSystem.CurrentLevel.ToString()));
            statistic.Add(FormatStatisticLine("Секций получено:", (gameContext.SnakeController.SegmentsBase.Count - 1).ToString()));
            statistic.Add(FormatStatisticLine("Яблок съедено:", collectableHandler.HealItemUsed.ToString()));
            statistic.Add(FormatStatisticLine("Магнитов использовано:", collectableHandler.MagnetItemUsed.ToString()));
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