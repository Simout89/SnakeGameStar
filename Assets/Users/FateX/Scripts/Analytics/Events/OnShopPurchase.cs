using Unity.Services.Analytics;

namespace Users.FateX.Scripts.Analytics.Events
{
    public class OnShopPurchase: Event
    {
        public OnShopPurchase(
            string purchaseName
        ) : base("onShopPurchase")
        {
            SetParameter("purchaseName", purchaseName);
        }
    }
}