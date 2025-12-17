using Unity.Services.Analytics;

namespace Users.FateX.Scripts.Analytics.Events
{
    public class OnDeathViewChooseButton: Event
    {
        public OnDeathViewChooseButton(
            string itemName
        ) : base("deathViewChooseButton")
        {
            SetParameter("itemName", itemName);
        }
    }
}