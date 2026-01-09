using Unity.Services.Analytics;

namespace Users.FateX.Scripts.Analytics.Events
{
    public class OnRunEndedEvent : Event
    {
        public OnRunEndedEvent(
            int lifeTime,
            int segmentsCounts,
            int userLevel,
            int coinsEarned,
            int onEnemyKilled
        ) : base("onRunEnded")
        {
            SetParameter("lifeTime", lifeTime);
            SetParameter("segmentsCounts", segmentsCounts);
            SetParameter("userLevel", userLevel);
            SetParameter("CoinsEarned", coinsEarned);
            SetParameter("onEnemyKilled", onEnemyKilled);
        }
    }
}