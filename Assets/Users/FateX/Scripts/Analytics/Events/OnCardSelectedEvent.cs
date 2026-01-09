using Unity.Services.Analytics;

namespace Users.FateX.Scripts.Analytics.Events
{
    public class OnCardSelectedEvent: Event
    {
        public OnCardSelectedEvent(
            string cardName
        ) : base("onCardSelected")
        {
            SetParameter("cardName", cardName);
        }
    }
}