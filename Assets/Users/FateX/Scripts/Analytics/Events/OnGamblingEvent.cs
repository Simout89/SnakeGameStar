using Unity.Services.Analytics;

namespace Users.FateX.Scripts.Analytics.Events
{
    public class OnGamblingEvent: Event
    {
        public OnGamblingEvent(
            string itemName
        ) : base("gambling")
        {
            SetParameter("itemName", itemName);
        }
    }
}