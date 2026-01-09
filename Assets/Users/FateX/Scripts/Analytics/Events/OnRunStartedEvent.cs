using Unity.Services.Analytics;

namespace Users.FateX.Scripts.Analytics.Events
{
    public class OnRunStartedEvent : Event
    {
        public OnRunStartedEvent(
        ) : base("onRunStarted")
        {
        }
    }
}