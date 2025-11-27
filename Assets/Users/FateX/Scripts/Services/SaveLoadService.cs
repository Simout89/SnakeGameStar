namespace Скриптерсы.Services
{
    public class SaveLoadService: ISaveLoadService
    {
        public PlayerCurrencyData LoadCurrencyData()
        {
            return new PlayerCurrencyData(0);
        }
    }

    public interface ISaveLoadService
    {
        public PlayerCurrencyData LoadCurrencyData();
    }
}