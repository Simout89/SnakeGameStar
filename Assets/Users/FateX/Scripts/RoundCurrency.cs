using System;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts
{
    public class RoundCurrency: IDisposable
    {
        [Inject] private CurrencyService _currencyService;
        public int Coin {get; private set; }

        public event Action<int> OnCoinChanged;

        public void AddCoin(int count)
        {
            if(count > 0)
            {
                Coin += count;
                OnCoinChanged?.Invoke(Coin);
            }
        }

        public void Dispose()
        {
            _currencyService.AddCoins(Coin);
        }
    }
}