using System.Collections.Generic;
using Users.FateX.Scripts;
using Users.FateX.Scripts.CollectableItem;
using Users.FateX.Scripts.Enemys;
using Users.FateX.Scripts.Utils;
using Zenject;

namespace Скриптерсы.Services
{
    public class StatisticsService: IStatisticsService
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
            statistic.Add($"Прожил времени: {MyUtils.FormatSeconds(gameTimer.CurrentTime)}");
            statistic.Add($"Монет получено: {roundCurrency.Coin}");
            statistic.Add($"Врагов убито: {enemyDeathHandler.TotalEnemyDie}");
            statistic.Add($"Уровней получено: {experienceSystem.CurrentLevel}");
            statistic.Add($"Секций получено: {gameContext.SnakeController.SegmentsBase.Count - 1}");
            statistic.Add($"Яблок съедено: {collectableHandler.HealItemUsed}");
            statistic.Add($"Магнитов использовано: {collectableHandler.MagnetItemUsed}");
            return statistic.ToArray();
        }
    }

    public interface IStatisticsService
    {
        public string[] GetStatistics();
    }
}