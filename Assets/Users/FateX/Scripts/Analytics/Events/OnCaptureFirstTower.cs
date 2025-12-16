using Unity.Services.Analytics;

namespace Users.FateX.Scripts.Analytics.Events
{
    public class OnCaptureFirstTower : Event
    {
        public OnCaptureFirstTower(
        ) : base("captureFirstTower")
        {
        }
    }
}