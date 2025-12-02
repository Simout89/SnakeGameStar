using System;
using UnityEngine;
using Zenject;

namespace Скриптерсы.Services
{
    public class CurrencyService: ICurrencyService
    {
        [Inject] private ISaveLoadService _saveLoadService;
        private readonly PlayerCurrencyData _playerCurrencyData;

        public int Coins
        {
            get
            {
                return _playerCurrencyData.Coins;
            }
        }

        public event Action<int> OnCoinsChanged;

        public CurrencyService(ISaveLoadService save)
        {
            _playerCurrencyData = save.LoadCurrencyData();
        }

        public void Reset()
        {
            _playerCurrencyData.Coins = 0;
        }
        
        public void AddCoins(int amount)
        {
            _playerCurrencyData.Coins += amount;
            OnCoinsChanged?.Invoke(_playerCurrencyData.Coins);

            _saveLoadService.SaveCurrencyData(_playerCurrencyData);
            Debug.Log("Добавлены монеты");
        }

        public bool TrySpendCoins(int amount)
        {
            if (_playerCurrencyData.Coins < amount)
            {
                return false;
            }
            else
            {
                _playerCurrencyData.Coins -= amount;
                OnCoinsChanged?.Invoke(_playerCurrencyData.Coins);
                _saveLoadService.SaveCurrencyData(_playerCurrencyData);
                return true;
            }
        }
    }
    
    public class PlayerCurrencyData
    {
        public int Coins;

        public PlayerCurrencyData(int count)
        {
            Coins = count;
        }
    }
    
    public interface ICurrencyService
    {
        int Coins { get; }
        event Action<int> OnCoinsChanged;
    
        void AddCoins(int amount);
        bool TrySpendCoins(int amount);
    }
}